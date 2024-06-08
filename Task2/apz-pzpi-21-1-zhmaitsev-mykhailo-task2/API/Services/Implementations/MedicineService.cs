using API.Core.Contexts;
using API.Core.DTOs.Medicine;
using API.Core.DTOs.Prescription;
using API.Core.Entities;
using API.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class MedicineService(OncoBoundDbContext context) : IMedicineService
{
    public async Task<List<MedicineDto>> GetMedicinesAsync()
    {
        return await context.Medicines.Select(m => new MedicineDto
        {
            MedicineId = m.Id,
            Name = m.Name,
            Description = m.Description,
            SideEffects = m.SideEffects,
            Interactions = m.Interactions
        }).ToListAsync();
    }

    public async Task<MedicineDto?> GetMedicineByIdAsync(int medicineId)
    {
        return await context.Medicines.Where(m => m.Id == medicineId)
            .Select(m => new MedicineDto
            {
                MedicineId = m.Id,
                Name = m.Name,
                Description = m.Description,
                SideEffects = m.SideEffects,
                Interactions = m.Interactions
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddMedicineAsync(MedicineCreateDto medicineCreateDto)
    {
        var medicine = new Medicine
        {
            Name = medicineCreateDto.Name,
            Description = medicineCreateDto.Description,
            SideEffects = medicineCreateDto.SideEffects,
            Interactions = medicineCreateDto.Interactions
        };
        
        await context.Medicines.AddAsync(medicine);
        await context.SaveChangesAsync();
        return medicine.Id;
    }

    public async Task<bool> UpdateMedicineAsync(int medicineId, MedicineUpdateDto updatedMedicine)
    {
        var existingMedicine = await context.Medicines.FindAsync(medicineId);

        if (existingMedicine == null)
            return false;

        existingMedicine.Name = updatedMedicine.Name;
        existingMedicine.Description = updatedMedicine.Description;
        existingMedicine.SideEffects = updatedMedicine.SideEffects;
        existingMedicine.Interactions = updatedMedicine.Interactions;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMedicineAsync(int medicineId)
    {
        var medicine = await context.Medicines.FindAsync(medicineId);

        if (medicine == null)
            return false;

        context.Medicines.Remove(medicine);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PrescriptedMedicineDto>> GetPrescriptedMedicines(int userId)
    {
        var currentDateUTC = DateTime.UtcNow.Date;
        
        var prescriptedMedicines = (await context.Prescriptions
                .Include(x => x.Medications)
                .Where(p => p.UserId == userId && p.DatePrescribedUTC != null)
                .ToListAsync())
            .SelectMany(p => p.Medications
                .Select(m => new PrescriptedMedicineDto(
                    p.Id,
                    m.MedicineId,
                    p.Dosage,
                    m.Frequency,
                    p.Duration,
                    p.DatePrescribedUTC!.Value
                )))
            .Where(dto => dto.Duration > 0 && dto.DatePrescribedUTC.AddDays(dto.Duration) >= currentDateUTC)
            .Where(dto => dto.Frequency > 0) // Frequency validation
            .ToList();

        var resPrescripterMedicines = new List<PrescriptedMedicineDto>(prescriptedMedicines);
        foreach (var medicationDto in prescriptedMedicines)
        {
            var medicine = await context.Medicines.FirstOrDefaultAsync(m => m.Id == medicationDto.MedicationId);
            if (medicine?.ExpirationDate < DateTime.Now)
            {
                var doctorsWithMedicineAsPrescriptions = await context.Doctors.Include(d => d.Prescriptions)
                    .Where(d => d.Prescriptions != null).ToListAsync();

                foreach (var doctor in doctorsWithMedicineAsPrescriptions)
                {
                    var notification = new Notification()
                    {
                        DoctorId = doctor.Id,
                        isRead = false,
                        Message = $"The medicine ${medicine.Name} with the number ${medicine.Id} has expired. Replace the medicine or change the prescription.",
                    };

                    context.Notifications.Add(notification);
                }
            }
                
            var medicationLogsCount = context.MedicationLogs
                .Count(log => log.MedicationId == medicationDto.MedicationId
                              && log.UserId == userId
                              && log.TimestampUTC.Date == currentDateUTC.Date);

            if (medicationLogsCount >= medicationDto.Frequency)
            {
                // ignore in case the medication was already taken for maximum frequency
                resPrescripterMedicines.Remove(medicationDto);
                continue;
            }
            
            // Log medication taken
            var logEntry = new MedicationLog
            {
                UserId = userId,
                MedicationId = medicationDto.MedicationId,
                TimestampUTC = DateTime.UtcNow,
                Status = "Taken"
            };

            context.MedicationLogs.Add(logEntry);
        }

        if (prescriptedMedicines.Count > 0 && resPrescripterMedicines.Count < 1)
        {
            // error in case medications was already taken for maximum frequency
            throw new FieldAccessException();
        }

        await context.SaveChangesAsync();

        return resPrescripterMedicines;
    }
}
