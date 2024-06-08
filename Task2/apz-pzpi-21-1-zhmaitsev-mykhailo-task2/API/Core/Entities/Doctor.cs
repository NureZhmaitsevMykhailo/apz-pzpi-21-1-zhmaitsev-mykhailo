namespace API.Core.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; }
    public string Specialty { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}