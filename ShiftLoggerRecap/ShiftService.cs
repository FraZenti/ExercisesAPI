using Microsoft.EntityFrameworkCore;

namespace ShiftLoggerRecap;

public class ShiftService
{
    private readonly ShiftLoggerContext _context;

    public ShiftService (ShiftLoggerContext context)
    {
        _context = context;
    }

    public async Task<List<Worker>> WorkersList()
    {
        var workers = await _context.Workers.ToListAsync();
        return workers;
    }
}