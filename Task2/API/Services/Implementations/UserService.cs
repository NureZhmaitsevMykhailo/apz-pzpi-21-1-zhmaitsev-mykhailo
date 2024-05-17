using API.Core.Contexts;
using API.Core.DTOs.User;
using API.Core.Entities;
using API.Services.Abstractions;
using API.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class UserService(OncoBoundDbContext context) : IUserService
{
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task CreateUserAsync(CreateUserDto user)
    {
        if (user == null)
        {
            throw new ArgumentException(nameof(user));
        }
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(user.Password);
        
        context.Users.Add(new User
        {
            Email = user.Email,
            Password = hashedPassword,
            Salt = salt, 
            Fullname= user.FullName,
        });
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto user)
    {
        if (user == null)
        {
            throw new ArgumentException(nameof(user));
        }

        var userDb = await context.FindAsync<User>(id);
        
        if (userDb == null)
        {
            throw new ArgumentException(nameof(user));
        }
        
        userDb.Email = user.Email;
        userDb.Fullname = user.Fullname;
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(user.Password);

        userDb.Password = hashedPassword;
        userDb.Salt = salt;
        
        context.Users.Update(userDb);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<User?> LogInAsync(string email, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user != null && PasswordHelper.IsPasswordValid(password, user.Password, user.Salt))
        {
            return user;
        }

        return null;
    }

    public async Task<bool> SignUpAsync(UserSignUpDto userSignUpDto)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == userSignUpDto.Email);

        if (existingUser != null)
        {
            return false;
        }
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(userSignUpDto.Password);
        
        var newUser = new User
        {
            Email = userSignUpDto.Email,
            Password = hashedPassword,
            Salt = salt,
            Fullname = userSignUpDto.Fullname,
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return true;
    }
}
