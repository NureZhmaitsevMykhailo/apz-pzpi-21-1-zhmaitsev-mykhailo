using API.Core.DTOs.Prescription;

namespace API.Services.Abstractions;

public interface IPrescriptionService
{
    Task<List<PrescriptionDto>> GetUserPrescriptionsAsync(int userId);
    Task<PrescriptionDto?> GetPrescriptionByIdAsync(int prescriptionId);
    Task<int> AddPrescriptionAsync(int userId, PrescriptionCreateDto prescriptionDto);
    Task<bool> UpdatePrescriptionAsync(int prescriptionId, PrescriptionUpdateDto updatedPrescriptionDto);
    Task<bool> DeletePrescriptionAsync(int prescriptionId);
    Task VerifyPrescription(int doctorId, VerifyPrescriptionDto verifyPrescriptionDto);
}