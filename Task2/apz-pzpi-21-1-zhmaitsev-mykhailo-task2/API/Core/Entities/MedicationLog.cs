namespace API.Core.Entities;

public class MedicationLog : BaseEntity
{
    public int MedicationId { get; set; }
    public Medication Medication { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime TimestampUTC { get; set; }
    public string Status { get; set; }
}