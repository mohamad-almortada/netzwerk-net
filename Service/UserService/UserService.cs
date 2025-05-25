using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Service.UserService;

public class UserService(ApiContext apiContext, ILogger<UserService> logger, IMapper mapper) : IUserService
{
    public async Task<User> CreateUserAsync(UsersDto userDto)
    {
        var myUser = mapper.Map<User>(userDto);
        
        if (!IsEmailUnique(myUser.Email)) throw new DbUpdateException("Email already in use");
        
        
        await apiContext.Users.AddAsync(myUser);
        await apiContext.SaveChangesAsync();
        return myUser;
    }


    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await apiContext.Users.FindAsync(userId);
        if (user == null) return false;
        
        apiContext.Users.Remove(user);
        await apiContext.SaveChangesAsync();
        return true;
    }

    public async Task<UsersDto?> GetUserAsync(int userId)
    {
        var user = await apiContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId);
        
        if (user == null) return null;
        
        var userDto = mapper.Map<UsersDto>(user);
        
        
        return userDto;
    }

    public async Task<UsersDto?> GetUserAsync(decimal latitude, decimal longitude)
    {
        var user = (await apiContext.Markers
            .Include(u => u.User)
            .SingleOrDefaultAsync(u => u.Lat == latitude && u.Lon == longitude))?.User;
        
        if (user == null) return null;
        
        var userDto = mapper.Map<UsersDto>(user);
        
        
        return userDto;
    }


    public Task<User> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UsersDto> UpdateUserAsync(int userId, UsersDto userDto)
    {
        throw new NotImplementedException();
    }


    public Task<IEnumerable<UsersDto>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }


    private bool IsEmailUnique(string email)
    {
        return !apiContext.Users.Any(u => u.Email == email);
    }
}