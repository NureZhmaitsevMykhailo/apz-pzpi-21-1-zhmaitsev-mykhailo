using API.Core.DTOs.Medicine;

namespace API.Core.DTOs.Medication;

public record PrescriptionMedicationDto
{
    public MedicineDto Medicine { get; set; }
    public int Frequency { get; set; }
    public DateTime? StartTimeUTC { get; set; }
    public DateTime? EndTimeUTC { get; set; }
}