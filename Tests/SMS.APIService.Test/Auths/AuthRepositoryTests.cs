using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Entities.Users;
using SMS.ApiService.Persistence;
using SMS.ApiService.Repositories.Auths;
using SMS.Common.Dtos.Users;


public class AuthRepositoryTests
{
    private readonly IConfiguration _configuration;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public AuthRepositoryTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "AppSettings:Token", "SMSSuperSecureAndRandomKeyThatLooksJustAwesomeAndNeedsToBeVeryVeryLong!!!111oneeleven" },
            { "AppSettings:Issuer", "SMSApp" },
            { "AppSettings:Audience", "SMSAudience" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task LoginAsync_ReturnsFailure_WhenUserNotFound()
    {
        using var context = new AppDbContext(_dbContextOptions);
        var repo = new AuthRepository(context, _configuration);
        var userDto = new UserDto { UserName = "nonexistent@example.com", Password = "password" };

        var result = await repo.LoginAsync(userDto);

        Assert.False(result.IsSuccess);
        Assert.Equal("User was not found", result.ErrorMessage);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_ReturnsFailure_WhenPasswordIsInvalid()
    {
        using var context = new AppDbContext(_dbContextOptions);
        var role = new Role
        {
            Id = "admin-role-id",
            Name = "Admin",
            Description = "Administrator role"  
        };
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@example.com",
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "correct-password"),
            Description = "System User",
            FirstName = "John",
            SurName = "Doe",
            RoleId = role.Id,
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new AuthRepository(context, _configuration);
        var userDto = new UserDto { UserName = "test@example.com", Password = "wrong-password" };

        var result = await repo.LoginAsync(userDto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid password", result.ErrorMessage);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_ReturnsSuccess_WhenCredentialsAreCorrect()
    {
        using var context = new AppDbContext(_dbContextOptions);

        var role = new Role
        {
            Id = "admin-role-id",
            Name = "Admin",
            Description = "Administrator role"  
        };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@example.com",
            PasswordHash = new PasswordHasher<User>().HashPassword(null, "correct-password"),
            FirstName = "John",
            SurName = "Doe",
            RoleId = role.Id,
            Description = "System User"  
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new AuthRepository(context, _configuration);
        var userDto = new UserDto { UserName = "test@example.com", Password = "correct-password" };

        var result = await repo.LoginAsync(userDto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data?.AccessToken);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

}
