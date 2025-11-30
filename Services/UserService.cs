using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;

namespace Netzwerk.Services;

public class UserService(ApiContext apiContext, ILogger<UserService> logger, IMapper mapper) : IUserService
{
   public async Task<User> CreateUserAsync(UsersDto userDto)
{
    var myUser = mapper.Map<User>(userDto);

    if (!IsEmailUnique(myUser.Email))
        throw new DbUpdateException("Email already in use");

    myUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

    myUser.CreatedAt = DateTime.UtcNow;

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

    public async Task<UsersDto?> GetUserAsync(string latitude, string longitude)
    {
        var user = (await apiContext.Markers
            .Include(u => u.User)
            .SingleOrDefaultAsync(u => u.Lat == latitude && u.Lon == longitude))?.User;

        if (user == null) return null;

        var userDto = mapper.Map<UsersDto>(user);


        return userDto;
    }


    public async Task<UsersDto?> GetUserByEmailAsync(string email)
    {
        var user = await apiContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        var userDto = mapper.Map<UsersDto>(user);
        return userDto;
    }

    public async Task<UsersDto?> UpdateUserAsync(int userId, UsersDto userDto)
    {
        var user = await apiContext.Users.FindAsync(userId);
        if (user == null) return null;

        user.Username = userDto.Username;
        user.Email = userDto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await apiContext.SaveChangesAsync();
        return userDto;
    }


    public async Task<IEnumerable<UsersDto>> GetUsersAsync()
    {
        var users = await apiContext.Users.ToListAsync();
        var usersDto = mapper.Map<IEnumerable<UsersDto>>(users);
        return usersDto;
    }


    private bool IsEmailUnique(string email)
    {
        return !apiContext.Users.Any(u => u.Email == email);
    }
}