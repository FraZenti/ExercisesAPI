using System.Text.Json.Serialization;

namespace ShiftLoggerRecap;

public class Shift
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int WorkerId { get; set; }

    [JsonIgnore]
    public Worker? Worker { get; set; }
}