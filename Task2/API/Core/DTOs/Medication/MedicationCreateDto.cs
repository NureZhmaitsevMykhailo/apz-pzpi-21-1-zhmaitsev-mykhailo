namespace API.Core.DTOs.Medication;

public record MedicationCreateDto
{
    public int MedicineId { get; set; }
    public int PrescriptionId { get; set; }
    public int Frequency { get; set; }
    public DateTime StartTimeUTC { get; set; }
    public DateTime EndTimeUTC { get; set; }
}