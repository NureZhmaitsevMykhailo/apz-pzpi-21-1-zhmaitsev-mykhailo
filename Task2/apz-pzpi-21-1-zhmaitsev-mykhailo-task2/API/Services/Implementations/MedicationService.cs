using API.Core.Contexts;
using API.Core.DTOs.Medication;
using API.Core.Entities;
using API.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class MedicationService(OncoBoundDbContext context) : IMedicationService
{
    public async Task<List<MedicationDto>> GetMedicationsAsync()
    {
        return await context.Medications
            .Select(m => new MedicationDto
            {
                MedicineId = m.MedicineId,
                Frequency = m.Frequency,
                StartTimeUTC = m.StartTimeUTC,
                EndTimeUTC = m.EndTimeUTC,
                PrescriptionId = m.PrescriptionId
            })
            .ToListAsync();
    }

    public async Task<MedicationDto?> GetMedicationByIdAsync(int medicineId)
    {
        return await context.Medications
            .Where(m => m.MedicineId == medicineId)
            .Select(m => new MedicationDto
            {
                MedicineId = m.MedicineId,
                Frequency = m.Frequency,
                StartTimeUTC = m.StartTimeUTC,
                EndTimeUTC = m.EndTimeUTC,
                PrescriptionId = m.PrescriptionId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddMedicationAsync(MedicationCreateDto medicationDto)
    {
        var medication = new Medication
        {
            MedicineId = medicationDto.MedicineId,
            Frequency = medicationDto.Frequency,
            StartTimeUTC = medicationDto.StartTimeUTC,
            EndTimeUTC = medicationDto.EndTimeUTC,
            PrescriptionId = medicationDto.PrescriptionId
        };

        await context.Medications.AddAsync(medication);
        await context.SaveChangesAsync();

        return medication.MedicineId;
    }

    public async Task<bool> UpdateMedicationAsync(int medicineId, MedicationUpdateDto updatedMedicationDto)
    {
        var existingMedication = await context.Medications.FindAsync(medicineId);

        if (existingMedication == null)
            return false;

        existingMedication.Frequency = updatedMedicationDto.Frequency;
        existingMedication.StartTimeUTC = updatedMedicationDto.StartTimeUTC;
        existingMedication.EndTimeUTC = updatedMedicationDto.EndTimeUTC;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMedicationAsync(int medicineId)
    {
        var medication = await context.Medications.FindAsync(medicineId);

        if (medication == null)
            return false;

        context.Medications.Remove(medication);
        await context.SaveChangesAsync();
        return true;
    }
}
