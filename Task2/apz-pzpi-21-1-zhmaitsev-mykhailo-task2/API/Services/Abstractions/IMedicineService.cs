using API.Core.DTOs.Medicine;
using API.Core.DTOs.Prescription;

namespace API.Services.Abstractions;

public interface IMedicineService
{
    Task<List<MedicineDto>> GetMedicinesAsync();
    Task<MedicineDto?> GetMedicineByIdAsync(int medicineId);
    Task<int> AddMedicineAsync(MedicineCreateDto medicineCreateDto);
    Task<bool> UpdateMedicineAsync(int medicineId, MedicineUpdateDto updatedMedicine);
    Task<bool> DeleteMedicineAsync(int medicineId);
    Task<List<PrescriptedMedicineDto>> GetPrescriptedMedicines(int userId);
}