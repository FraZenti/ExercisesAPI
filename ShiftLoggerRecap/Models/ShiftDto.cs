namespace ShiftLoggerRecap;

public class ShiftDto
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int WorkerId { get; set; }

    public string? WorkerName { get; set; }
}