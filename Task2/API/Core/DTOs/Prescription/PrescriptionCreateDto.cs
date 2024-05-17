using API.Core.DTOs.Medicine;

namespace API.Core.DTOs.Prescription;

public record PrescriptionCreateDto
{
    public int Dosage { get; set; }
    public int Duration { get; set; }
    public List<AddPrescriptionMedicineDto> Medicines { get; set; }
}