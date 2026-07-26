using ECommerce.Data;
using ECommerce.Dtos;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace ECommerce.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthContext _user;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _service;
        public AuthService (AuthContext userContext, IConfiguration configuration, IServiceProvider service)
        { 
            _user = userContext;
            _configuration = configuration;
            _service = service;
        }

        public async Task<bool> RegisterAsync(UserDto dto)
        {
            if (_user.User.Any(x => x.Username == dto.Username)) return false;

            var user = new UserModel();
            var hashedPassword = new PasswordHasher<UserModel>()
                .HashPassword(user, dto.Password);

            user.Username = dto.Username;
            user.HashedPassword = hashedPassword;
            user.Role = "Buyer";

            await _user.User.AddAsync(user);

            await _user.SaveChangesAsync();
            return true;
        }
        public async Task<TokenResponseDto?> LoginAsync(UserDto dto)
        {
            var user = await _user.User.FirstOrDefaultAsync(x => x.Username == dto.Username);
            if (user is null) return null;

            if (new PasswordHasher<UserModel>()
                .VerifyHashedPassword(user, user.HashedPassword, dto.Password) == PasswordVerificationResult.Failed) return null;

            return await CreateTokenResponse(user);
        }
        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)  
        {
            var user = await RefreshTokenValidator(dto.UserId, dto.RefreshToken);
            if (user is null) return null;

            return await CreateTokenResponse(user);
        }
        //==========================================================================================================

        private async Task<TokenResponseDto> CreateTokenResponse(UserModel user)
        {
            return new TokenResponseDto
            {
                UserId = user.id,
                AccessToken = await CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
        //==========================================================================================================
        private async Task<UserModel?> RefreshTokenValidator(int userId, string refreshToken)
        {
            var user = await _user.User.FindAsync(userId);
            if (user == null || user.RefreshTokenExpiry <= DateTime.Now || refreshToken != user.RefreshToken) return null;


            if (user.RefreshToken != refreshToken) return null;

            return user;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(UserModel user)
        {
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(11);

            await _user.SaveChangesAsync();
            return refreshToken;
        }
        //==========================================================================================================
        private async Task<string> CreateToken(UserModel user)  //check if there's an error because i'm returning a string
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("jwtsettings:Token")!));

            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, SecurityAlgorithms.HmacSha512); //choose the encryption type

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("jwtsettings:Issuer"),
                audience: _configuration.GetValue<string>("jwtsettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
