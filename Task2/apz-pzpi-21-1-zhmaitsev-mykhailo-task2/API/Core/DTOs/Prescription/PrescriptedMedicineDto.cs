namespace API.Core.DTOs.Prescription;

public record PrescriptedMedicineDto(int PrescriptionId, int MedicationId, int Dosage, int Frequency, int Duration, DateTime DatePrescribedUTC);

public record PrescriptedMedicineDtoList(List<PrescriptedMedicineDto> PrescriptedMedicineDtos);
