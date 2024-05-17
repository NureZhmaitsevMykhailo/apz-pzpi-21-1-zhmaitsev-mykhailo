using API.Core.Contexts;
using API.Core.DTOs.Medication;
using API.Core.DTOs.Medicine;
using API.Core.DTOs.Prescription;
using API.Core.Entities;
using API.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations;

public class PrescriptionService(OncoBoundDbContext context): IPrescriptionService
{
    public async Task<List<PrescriptionDto>> GetUserPrescriptionsAsync(int userId)
    {
        var prescriptions = await context.Prescriptions
            .Include(x => x.Medications)
            .Where(x => x.UserId == userId)
            .Select(p => new PrescriptionDto
            {
                Id = p.Id,
                DatePrescribedUTC = p.DatePrescribedUTC,
                UserId = p.UserId,
                DoctorId = p.DoctorId,
                Dosage = p.Dosage,
                Duration = p.Duration,
                Medications = context.Medications
                    .Include(m => m.Medicine)
                    .Where(x => x.PrescriptionId == p.Id)
                    .Select(x => new PrescriptionMedicationDto
                    {
                        Medicine = new MedicineDto
                        {
                            MedicineId = x.MedicineId,
                            Name = x.Medicine.Name,
                            Description = x.Medicine.Description,
                            SideEffects = x.Medicine.SideEffects,
                            Interactions = x.Medicine.Interactions
                        },
                        Frequency = x.Frequency,
                        StartTimeUTC = x.StartTimeUTC,
                        EndTimeUTC = x.EndTimeUTC
                    })
                    .ToList()
            })
            .ToListAsync();

        return prescriptions;
    }

    public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int prescriptionId)
    {
        return await context.Prescriptions
            .Where(p => p.Id == prescriptionId)
            .Select(p => new PrescriptionDto
            {
                Id = p.Id,
                DatePrescribedUTC = p.DatePrescribedUTC,
                UserId = p.UserId,
                DoctorId = p.DoctorId,
                Dosage = p.Dosage,
                Duration = p.Duration,
                Medications = context.Medications
                    .Include(m => m.Medicine)
                    .Select(x => new PrescriptionMedicationDto
                    {
                        Medicine = new MedicineDto
                        {
                            MedicineId = x.MedicineId,
                            Name = x.Medicine.Name,
                            Description = x.Medicine.Description,
                            SideEffects = x.Medicine.SideEffects,
                            Interactions = x.Medicine.Interactions
                        },
                        Frequency = x.Frequency,
                        StartTimeUTC = x.StartTimeUTC,
                        EndTimeUTC = x.EndTimeUTC
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddPrescriptionAsync(int userId, PrescriptionCreateDto prescriptionDto)
    {
        foreach (var medicine in prescriptionDto.Medicines)
        {
            await context.Medicines.FirstAsync(x => x.Id == medicine.MedicineId); // throws an exception if medicine doesn't exist
        }
        
        var prescription = new Prescription
        {
            UserId = userId,
            Dosage = prescriptionDto.Dosage,
            Duration = prescriptionDto.Duration
        };

        await context.Prescriptions.AddAsync(prescription);
        await context.SaveChangesAsync();

        foreach (var medicine in prescriptionDto.Medicines)
        {
            var medication = new Medication
            {
                PrescriptionId = prescription.Id,
                MedicineId = medicine.MedicineId,
                Frequency = medicine.Frequency
            };

            await context.Medications.AddAsync(medication);
        }
        
        await context.SaveChangesAsync();

        return prescription.Id;
    }

    public async Task<bool> UpdatePrescriptionAsync(int prescriptionId, PrescriptionUpdateDto updatedPrescriptionDto)
    {
        var existingPrescription = await context.Prescriptions.FindAsync(prescriptionId);

        if (existingPrescription == null)
            return false;

        existingPrescription.Dosage = updatedPrescriptionDto.Dosage;
        existingPrescription.Duration = updatedPrescriptionDto.Duration;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePrescriptionAsync(int prescriptionId)
    {
        var prescription = await context.Prescriptions.FindAsync(prescriptionId);

        if (prescription == null)
            return false;

        context.Prescriptions.Remove(prescription);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task VerifyPrescription(int doctorId, VerifyPrescriptionDto verifyPrescriptionDto)
    {
        var prescription = await context.Prescriptions
            .Include(prescription => prescription.Doctor)
            .FirstOrDefaultAsync(p => p.Id == verifyPrescriptionDto.prescriptionId);

        if (prescription is not {Doctor: null})
        {
            throw new ArgumentException(nameof(prescription));
        }

        if (verifyPrescriptionDto.dosage <= 0)
        {
            throw new ArgumentException(nameof(verifyPrescriptionDto.dosage));
        }
        
        if (verifyPrescriptionDto.duration <= 0)
        {
            throw new ArgumentException(nameof(verifyPrescriptionDto.duration));
        }

        var curDateTimeUtc = DateTime.UtcNow;
        
        prescription.DoctorId = doctorId;
        prescription.Dosage = verifyPrescriptionDto.dosage;
        prescription.Duration = verifyPrescriptionDto.duration;
        prescription.DatePrescribedUTC = curDateTimeUtc;

        context.Prescriptions.Update(prescription);

        foreach (var medication in context.Medications.Where(x => x.PrescriptionId == verifyPrescriptionDto.prescriptionId))
        {
            medication.StartTimeUTC = curDateTimeUtc;
            medication.EndTimeUTC = verifyPrescriptionDto.endTimeUtc;
            context.Medications.Update(medication);
        }
        
        await context.SaveChangesAsync();
    }
}
