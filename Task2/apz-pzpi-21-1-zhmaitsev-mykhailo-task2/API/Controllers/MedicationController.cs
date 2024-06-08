using API.Core.Attributes;
using API.Core.DTOs.Medication;
using API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/medication")]
[DoctorRoleInterceptor]
[Authorize]
public class MedicationController(IMedicationService medicationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MedicationDto>>> GetMedications()
    {
        var medications = await medicationService.GetMedicationsAsync();
        return Ok(medications);
    }

    [HttpGet("{medicineId:int}")]
    public async Task<ActionResult<MedicationDto>> GetMedicationById(int medicineId)
    {
        var medication = await medicationService.GetMedicationByIdAsync(medicineId);

        if (medication == null)
            return NotFound();

        return Ok(medication);
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddMedication([FromBody] MedicationCreateDto medicationDto)
    {
        var addedMedicationId = await medicationService.AddMedicationAsync(medicationDto);
        return Ok(addedMedicationId);
    }

    [HttpPut("{medicineId:int}")]
    public async Task<ActionResult<bool>> UpdateMedication(int medicineId, [FromBody] MedicationUpdateDto updatedMedicationDto)
    {
        var result = await medicationService.UpdateMedicationAsync(medicineId, updatedMedicationDto);

        if (!result)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{medicineId:int}")]
    public async Task<ActionResult<bool>> DeleteMedication(int medicineId)
    {
        var result = await medicationService.DeleteMedicationAsync(medicineId);

        if (!result)
            return NotFound();

        return Ok(result);
    }
}
