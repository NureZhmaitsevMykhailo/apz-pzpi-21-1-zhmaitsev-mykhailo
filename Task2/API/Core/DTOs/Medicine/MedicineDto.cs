namespace API.Core.DTOs.Medicine;

public class MedicineDto
{
    public int MedicineId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SideEffects { get; set; }
    public string Interactions { get; set; }
}
