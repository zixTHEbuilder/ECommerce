using ECommerce.Dtos;

namespace ECommerce.Services
{
    public interface IAuthService 
    {
        Task<bool> RegisterAsync(UserDto dto);
        Task<TokenResponseDto> LoginAsync(UserDto dto);
        Task<TokenResponseDto> RefreshToken(RefreshTokenRequestDto dto);
    }
}
