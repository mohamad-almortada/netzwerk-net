using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Service;

public class UserService : IUserService
{
    private readonly ApiContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly IGeoLocationService GeoLocationservice;

    public UserService(ApiContext apiContext, ILogger<UserService> logger, IGeoLocationService geoLocationService)
    {
        _context = apiContext;
        _logger = logger;
        GeoLocationservice = geoLocationService;
    }

    public async Task<User> CreateUserAsync(UsersDto userDto)
    {
        var myUser = UserFrom(userDto);

        if (!IsEmailUnique(myUser.Email))
        {
            throw new DbUpdateException("Email already in use");
        }

        foreach (var keywordId in userDto.KeywordIds)
        {
            var keyword = await _context.Keywords.AsTracking().SingleOrDefaultAsync(k => k.Id == keywordId);
            if (keyword != null)
            {
                myUser.UserKeywords.Add(new UserKeyword
                    { Keyword = keyword, KeywordId = keyword.Id, UserId = myUser.Id, User = myUser });
            }
        }

        await _context.Users.AddAsync(myUser);
        await _context.SaveChangesAsync();
        return myUser;
    }


    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.Include(u => u.UserKeywords).SingleOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UsersDto> GetUserAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.UserKeywords)
            .SingleOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        var userDto = UserDtoFrom(user);

        userDto.KeywordIds = user.UserKeywords.Select(uk => uk.KeywordId).ToList();

        return userDto;
    }

    public async Task<UsersDto> GetUserAsync(string latitude, string longitude)
    {
        var user = await _context.Users
            .Include(u => u.UserKeywords)
            .SingleOrDefaultAsync(u => u.GeoLocationLat == latitude && u.GeoLocationLon == longitude);

        if (user == null) return null;

        var userDto = UserDtoFrom(user);

        userDto.KeywordIds = user.UserKeywords.Select(uk => uk.KeywordId).ToList();

        return userDto;
    }


    public Task<User> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<UsersDto> UpdateUserAsync(int userId, UsersDto userDto)
    {
        var myUser = await _context.Users.Include(u => u.UserKeywords).SingleOrDefaultAsync(u => u.Id == userId);
        if (myUser == null)
        {
            return null;
        }

        if (myUser.Email != userDto.Email && !IsEmailUnique(userDto.Email))
            throw new InvalidOperationException("Email already in use");

        myUser.FirstName = userDto.FirstName;
        myUser.LastName = userDto.LastName;
        myUser.Role = userDto.Role;
        myUser.Position = userDto.Position;
        myUser.Title = userDto.Title;
        myUser.SecondaryTitle = userDto.SecondaryTitle;
        myUser.PublicEmail = userDto.PublicEmail;
        myUser.Website = userDto.Website;
        myUser.Tel = userDto.Tel;
        myUser.Verified = userDto.Verified;
        myUser.VerifyToken = userDto.VerifyToken;
        myUser.Activities = userDto.Activities;
        myUser.AdditionalLinks = userDto.AdditionalLinks;
        myUser.FieldOfResearch = userDto.FieldOfResearch;
        myUser.ProfileImageSource = userDto.ProfileImageSource;
        myUser.Email = userDto.Email;
        if (userDto.GeoLocationLat != null && userDto.GeoLocationLon != null &&
            userDto.GeoLocationLat != myUser.GeoLocationLat && userDto.GeoLocationLon != myUser.GeoLocationLon)
        {
            var geolocation =
                await GeoLocationservice.GetGeoLocationAsync(userDto.GeoLocationLon, userDto.GeoLocationLat);
            if (geolocation == null) throw new UserServiceException("Provided latitude and longitude are invalid");
        }

        _logger.LogError("Provided latitude and longitude are AAFTER THAT IF U KNOW");
        myUser.GeoLocationLat = userDto.GeoLocationLat;
        myUser.GeoLocationLon = userDto.GeoLocationLon;

        myUser.UserKeywords.Clear();
        foreach (var keywordId in userDto.KeywordIds)
        {
            var keyword = await _context.Keywords.AsTracking().SingleOrDefaultAsync(k => k.Id == keywordId);
            if (keyword != null)
                myUser.UserKeywords.Add(new UserKeyword
                    { Keyword = keyword, KeywordId = keyword.Id, UserId = myUser.Id, User = myUser });
        }

        await _context.SaveChangesAsync();
        return userDto;
    }


    public async Task<IEnumerable<UsersDto>> GetUsersAsync()
    {
        var users = await _context.Users.Include(b => b.UserKeywords).ToListAsync();
        var userDtos = users.Select(u =>
        {
            var userDto = UserDtoFrom(u);
            userDto.KeywordIds = u.UserKeywords.Select(uk => uk.KeywordId).ToList();
            return userDto;
        });

        return userDtos;
    }

    private User UserFrom(UsersDto usersDto)
    {
        return new User
        {
            Id = usersDto.Id,
            FirstName = usersDto.FirstName,
            LastName = usersDto.LastName,
            Role = usersDto.Role,
            Position = usersDto.Position,
            Title = usersDto.Title,
            SecondaryTitle = usersDto.SecondaryTitle,
            PublicEmail = usersDto.PublicEmail,
            Website = usersDto.Website,
            Tel = usersDto.Tel,
            Verified = usersDto.Verified,
            VerifyToken = usersDto.VerifyToken,
            Activities = usersDto.Activities,
            AdditionalLinks = usersDto.AdditionalLinks,
            FieldOfResearch = usersDto.FieldOfResearch,
            ProfileImageSource = usersDto.ProfileImageSource,
            Email = usersDto.Email,
            GeoLocationLat = usersDto.GeoLocationLat,
            GeoLocationLon = usersDto.GeoLocationLon
        };
    }

    private UsersDto UserDtoFrom(User user)
    {
        return new UsersDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Position = user.Position,
            Title = user.Title,
            SecondaryTitle = user.SecondaryTitle,
            PublicEmail = user.PublicEmail,
            Website = user.Website,
            Tel = user.Tel,
            Verified = user.Verified,
            VerifyToken = user.VerifyToken,
            Activities = user.Activities,
            AdditionalLinks = user.AdditionalLinks,
            FieldOfResearch = user.FieldOfResearch,
            ProfileImageSource = user.ProfileImageSource,
            Email = user.Email,
            GeoLocationLat = user.GeoLocationLat,
            GeoLocationLon = user.GeoLocationLon
        };
    }

    private bool IsEmailUnique(string email)
    {
        return !_context.Users.Any(u => u.Email == email);
    }
}