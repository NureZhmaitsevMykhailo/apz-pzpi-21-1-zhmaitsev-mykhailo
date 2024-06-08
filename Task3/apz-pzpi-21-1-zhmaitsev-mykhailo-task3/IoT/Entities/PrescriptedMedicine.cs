using Newtonsoft.Json;

namespace IoT.Entities;

public class PrescriptedMedicine
{
    [JsonProperty("prescriptionId")]
    public int prescriptionId { get; init; }

    [JsonProperty("medicationId")]
    public int medicationId { get; init; }

    [JsonProperty("dosage")]
    public int dosage { get; init; }

    [JsonProperty("frequency")]
    public int frequency { get; init; }

    [JsonProperty("duration")]
    public int duration { get; init; }

    [JsonProperty("datePrescribedUTC")]
    public DateTime datePrescribedUTC { get; init; }
}
