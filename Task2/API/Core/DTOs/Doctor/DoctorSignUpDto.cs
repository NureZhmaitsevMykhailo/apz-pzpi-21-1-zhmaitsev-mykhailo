namespace API.Core.DTOs.Doctor;

public record DoctorSignUpDto(string Email, string Password, string Name, string Specialty);