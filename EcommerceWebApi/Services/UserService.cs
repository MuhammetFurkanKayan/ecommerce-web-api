using EcommerceWebApi.DTOs;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerceWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService( IGenericRepository<User> userRepository, IGenericRepository<Order> orderRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(ToDto);
        }

        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            return ToDto(user);
        }

        public async Task PatchAsync(int id, UserPatchDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            if (user.UserName != null)
            {
                if (await _userRepository.AnyAsync(u => u.UserName == user.UserName && u.Id != id))
                {
                    throw new InvalidOperationException("Username is already taken.");
                }
                existingUser.UserName = user.UserName;
            }
            if (user.Age.HasValue)
            {
                if (user.Age.Value < 0)
                {
                    throw new ArgumentException("Age cannot be negative.");
                }
                existingUser.Age = user.Age.Value;
            }
            if (user.Address != null)
            {
                existingUser.Address = user.Address;
            }

            await _userRepository.UpdateAsync(existingUser);
            await _userRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            if (await _orderRepository.AnyAsync(o => o.UserId == id))
            {
                throw new InvalidOperationException("User has orders and cannot be deleted.");
            }

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveAsync();
        }

        private static UserResponseDto ToDto(User user) => new()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Age = user.Age,
            Address = user.Address,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
        public async Task ChangePasswordAsync(int id, UpdateUserPasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Current password is incorrect.");
            }
            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new ArgumentException("New password cannot be empty.");
            }
            if (dto.CurrentPassword == dto.NewPassword)
            {
                throw new ArgumentException("New password must be different from current password.");
            }
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();
        }
        public async Task ChangeEmailAsync(int id, UpdateUserEmailDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Current password is incorrect.");
            }
            if (string.IsNullOrWhiteSpace(dto.NewEmail))
            {
                throw new ArgumentException("New email cannot be empty.");
            }
            if (user.Email == dto.NewEmail)
            {
                throw new ArgumentException("New email must be different from current email.");
            }
            if (await _userRepository.AnyAsync(u => u.Email == dto.NewEmail && u.Id != id))
            {
                throw new InvalidOperationException("Email is already in use.");
            }
            user.Email = dto.NewEmail;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();
        }
    }
}