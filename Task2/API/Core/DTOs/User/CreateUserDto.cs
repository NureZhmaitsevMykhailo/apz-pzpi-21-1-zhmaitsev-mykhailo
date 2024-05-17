namespace API.Core.DTOs.User;

public record CreateUserDto(string FullName, string Password, string Email);