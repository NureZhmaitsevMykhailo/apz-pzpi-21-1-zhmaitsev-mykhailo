using API.Core.DTOs.User;
using API.Core.Entities;

namespace API.Services.Abstractions;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserAsync(int id);
    Task CreateUserAsync(CreateUserDto user);
    Task UpdateUserAsync(int id, UpdateUserDto user);
    Task DeleteUserAsync(int id);
    Task<User?> LogInAsync(string email, string password);
    Task<bool> SignUpAsync(UserSignUpDto userSignUpDto);
}