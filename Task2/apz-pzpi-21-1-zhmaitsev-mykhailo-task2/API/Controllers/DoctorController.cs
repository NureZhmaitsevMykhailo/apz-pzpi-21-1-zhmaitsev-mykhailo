using API.Core.Constants;
using API.Core.DTOs.Doctor;
using API.Core.Entities;
using API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController(IDoctorService doctorService, IJwtService jwtService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
    {
        var doctors = await doctorService.GetDoctorsAsync();
        return Ok(doctors);
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Doctor>> GetDoctor(int id)
    {
        var doctor = await doctorService.GetDoctorAsync(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return Ok(doctor);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] CreateDoctorDto doctor)
    {
        try
        {
            await doctorService.CreateDoctorAsync(doctor);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto doctor)
    {
        try
        {
            await doctorService.UpdateDoctorAsync(id, doctor);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] DoctorLoginDto doctorLoginDto)
    {
        var doctor = await doctorService.LogInAsync(doctorLoginDto.Email, doctorLoginDto.Password);

        if (doctor == null)
        {
            return Unauthorized("Invalid email or password");
        }
        
        var token = jwtService.GenerateToken(doctor.Id, Role.DoctorRole);
        return Ok(token);
    }
    
    [HttpPost("signup")]
    public async Task<ActionResult> SignUp([FromBody] DoctorSignUpDto doctorSignUpDto)
    {
        try
        {
            var result = await doctorService.SignUpAsync(doctorSignUpDto);

            if (result)
            {
                return Ok("Doctor registration successful");
            }

            return BadRequest("Doctor with the same email already exists");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}
