namespace API.Core.Entities;

public class Notification : BaseEntity
{
    public string Message { get; set; }
    public bool isRead { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
}
