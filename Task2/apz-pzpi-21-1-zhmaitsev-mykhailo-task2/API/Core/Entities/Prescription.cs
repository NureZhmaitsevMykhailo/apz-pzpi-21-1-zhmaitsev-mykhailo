namespace API.Core.Entities;

public class Prescription : BaseEntity
{
    public DateTime? DatePrescribedUTC { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public int Dosage { get; set; }
    public int Duration { get; set; }
    public List<Medication> Medications { get; set; }
}