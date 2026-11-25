using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
    }
}
