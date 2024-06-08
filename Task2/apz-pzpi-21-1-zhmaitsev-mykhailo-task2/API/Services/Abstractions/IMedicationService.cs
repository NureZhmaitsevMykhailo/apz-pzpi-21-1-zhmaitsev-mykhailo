using API.Core.DTOs.Medication;

namespace API.Services.Abstractions;

public interface IMedicationService
{
    Task<List<MedicationDto>> GetMedicationsAsync();
    Task<MedicationDto?> GetMedicationByIdAsync(int medicineId);
    Task<int> AddMedicationAsync(MedicationCreateDto medicationDto);
    Task<bool> UpdateMedicationAsync(int medicineId, MedicationUpdateDto updatedMedicationDto);
    Task<bool> DeleteMedicationAsync(int medicineId);
}
