using API.Core.DTOs.Doctor;
using API.Core.Entities;

namespace API.Services.Abstractions;

public interface IDoctorService
{
    Task<IEnumerable<Doctor>> GetDoctorsAsync();
    Task<Doctor?> GetDoctorAsync(int id);
    Task CreateDoctorAsync(CreateDoctorDto doctor);
    Task UpdateDoctorAsync(int id, UpdateDoctorDto doctor);
    Task<Doctor?> LogInAsync(string email, string password);
    Task<bool> SignUpAsync(DoctorSignUpDto doctorSignUpDto);
}
