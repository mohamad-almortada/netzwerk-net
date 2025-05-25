using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(UsersDto userDto);
    Task<UsersDto?> GetUserAsync(int userId);
    Task<UsersDto?> GetUserAsync(decimal latitude, decimal longitude);
    Task<UsersDto?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UsersDto>> GetUsersAsync();
    Task<UsersDto?> UpdateUserAsync(int userId, UsersDto userDto);
    Task<bool> DeleteUserAsync(int userId);
}