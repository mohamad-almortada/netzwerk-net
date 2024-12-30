using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Service
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UsersDto userDto);
        Task<UsersDto> GetUserAsync(int userId);
        Task<UsersDto> GetUserAsync(string latitude, string longitude);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<UsersDto>> GetUsersAsync();
        Task<UsersDto> UpdateUserAsync(int userId, UsersDto userDto);
        Task<bool> DeleteUserAsync(int userId);
    }
}