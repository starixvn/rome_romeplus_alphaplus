namespace AlphaPlus.Core.Models;

public class Device
{
    public string Id { get; set; }

    public DeviceInfo Info { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}
