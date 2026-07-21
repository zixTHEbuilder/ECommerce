using ECommerce.Data;
using ECommerce.Dtos;

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

            
        }
        public async Task<TokenResponseDto> LoginAsync(UserDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenResponseDto> RefreshToken(RefreshTokenRequestDto dto)  
        {
            throw new NotImplementedException();
        }
    }
}
