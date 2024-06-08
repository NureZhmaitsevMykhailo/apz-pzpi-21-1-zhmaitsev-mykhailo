namespace API.Core.DTOs.Medication;

public class MedicationUpdateDto
{
    public int Frequency { get; set; }
    public DateTime StartTimeUTC { get; set; }
    public DateTime EndTimeUTC { get; set; }
}