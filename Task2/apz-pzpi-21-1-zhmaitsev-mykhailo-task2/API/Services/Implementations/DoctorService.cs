using API.Core.Contexts;
using API.Core.DTOs.Doctor;
using API.Core.Entities;
using API.Services.Abstractions;
using API.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class DoctorService(OncoBoundDbContext context) : IDoctorService
{
    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await context.Doctors.ToListAsync();
    }

    public async Task<Doctor?> GetDoctorAsync(int id)
    {
        return await context.Doctors.FindAsync(id);
    }

    public async Task CreateDoctorAsync(CreateDoctorDto doctor)
    {
        if (doctor == null)
        {
            throw new ArgumentException(nameof(doctor));
        }
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(doctor.Password);
        
        await context.Doctors.AddAsync(new Doctor
        {
            Email = doctor.Email,
            Password = hashedPassword,
            Salt = salt, 
            Name= doctor.Name,
            Specialty = doctor.Specialty
        });
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateDoctorAsync(int id, UpdateDoctorDto doctor)
    {
        if (doctor == null)
        {
            throw new ArgumentException(nameof(doctor));
        }

        var doctorDb = await context.FindAsync<Doctor>(id);
        
        if (doctorDb == null)
        {
            throw new ArgumentException(nameof(doctor));
        }

        doctorDb.Email = doctor.Email;
        doctorDb.Name = doctor.Name;
        doctorDb.Specialty = doctor.Specialty;
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(doctor.Password);

        doctorDb.Password = hashedPassword;
        doctorDb.Salt = salt;
        
        context.Doctors.Update(doctorDb);
        
        await context.SaveChangesAsync();
    }

    public async Task<Doctor?> LogInAsync(string email, string password)
    {
        var doctor = await context.Doctors.FirstOrDefaultAsync(u => u.Email == email);
        
        if (doctor != null && PasswordHelper.IsPasswordValid(password, doctor.Password, doctor.Salt))
        {
            return doctor;
        }

        return null;
    }

    public async Task<bool> SignUpAsync(DoctorSignUpDto doctorSignUpDto)
    {
        var existingDoctor = await context.Users.FirstOrDefaultAsync(u => u.Email == doctorSignUpDto.Email);

        if (existingDoctor != null)
        {
            return false;
        }
        
        var (hashedPassword, salt) = PasswordHelper.HashPassword(doctorSignUpDto.Password);
        
        var newDoctor = new Doctor
        {
            Email = doctorSignUpDto.Email,
            Password = hashedPassword,
            Salt = salt,
            Name = doctorSignUpDto.Name,
            Specialty = doctorSignUpDto.Specialty
        };

        context.Doctors.Add(newDoctor);
        await context.SaveChangesAsync();

        return true;
    }
}
