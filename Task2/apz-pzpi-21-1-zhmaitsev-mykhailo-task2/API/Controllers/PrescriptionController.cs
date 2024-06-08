using System.Security.Claims;
using API.Core.Attributes;
using API.Core.DTOs.Prescription;
using API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/prescription")]
[Authorize]
public class PrescriptionController(IPrescriptionService prescriptionService) : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<List<PrescriptionDto>>> GetMyPrescriptions()
    {
        var userId = Convert.ToInt32((HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);
        var prescriptions = await prescriptionService.GetUserPrescriptionsAsync(userId);
        return Ok(prescriptions);
    }

    [HttpGet("{prescriptionId:int}")]
    public async Task<ActionResult<PrescriptionDto>> GetPrescriptionById(int prescriptionId)
    {
        var prescription = await prescriptionService.GetPrescriptionByIdAsync(prescriptionId);

        if (prescription == null)
            return NotFound();

        return Ok(prescription);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> AddPrescription([FromBody] PrescriptionCreateDto prescriptionDto)
    {
        var userId = Convert.ToInt32((HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);
        var addedPrescriptionId = await prescriptionService.AddPrescriptionAsync(userId, prescriptionDto);
        return Ok(addedPrescriptionId);
    }

    [HttpPut("{prescriptionId:int}")]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<bool>> UpdatePrescription(int prescriptionId, [FromBody] PrescriptionUpdateDto updatedPrescriptionDto)
    {
        var result = await prescriptionService.UpdatePrescriptionAsync(prescriptionId, updatedPrescriptionDto);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{prescriptionId:int}")]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<bool>> DeletePrescription(int prescriptionId)
    {
        var result = await prescriptionService.DeletePrescriptionAsync(prescriptionId);

        if (!result)
            return NotFound();

        return Ok(result);
    }
    
    [HttpPost("verify")]
    [DoctorRoleInterceptor]
    public async Task<ActionResult<bool>> VerifyPrescription(VerifyPrescriptionDto verifyPrescriptionDto)
    {
        try
        {
            var doctorId = Convert.ToInt32((HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);
            await prescriptionService.VerifyPrescription(doctorId, verifyPrescriptionDto);
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
}
