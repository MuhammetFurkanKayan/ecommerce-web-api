using EcommerceWebApi.DTOs;

namespace EcommerceWebApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto> GetByIdAsync(int id);
        Task PatchAsync(int id, UserPatchDto user);
        Task DeleteAsync(int id);
        Task ChangePasswordAsync(int id, UpdateUserPasswordDto dto);
        Task ChangeEmailAsync(int id, UpdateUserEmailDto dto);
    }
}