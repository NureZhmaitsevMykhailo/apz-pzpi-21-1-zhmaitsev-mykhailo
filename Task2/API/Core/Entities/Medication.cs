namespace API.Core.Entities;

public class Medication : BaseEntity
{
    public int PrescriptionId { get; set; }
    public Prescription Prescription { get; set; }
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set; }
    public int Frequency { get; set; }
    public DateTime? StartTimeUTC { get; set; }
    public DateTime? EndTimeUTC { get; set; }
    public List<MedicationLog> MedicationLogs { get; set; }
}