namespace API.Core.DTOs.Prescription;

public record VerifyPrescriptionDto(int prescriptionId, int dosage, int duration, DateTime endTimeUtc);