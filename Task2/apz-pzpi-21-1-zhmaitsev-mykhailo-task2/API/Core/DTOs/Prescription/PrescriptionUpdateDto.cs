namespace API.Core.DTOs.Prescription;

public record PrescriptionUpdateDto
{
    public int Dosage { get; set; }
    public int Duration { get; set; }
}