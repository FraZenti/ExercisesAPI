namespace ShiftLoggerRecap;

public class WorkerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<WorkerShiftDto> Shifts { get; set; } = new List<WorkerShiftDto>();
}