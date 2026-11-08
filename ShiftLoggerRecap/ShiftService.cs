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
        var workers = await _context.Workers
            .ToListAsync();
        return workers;
    }

    public async Task<Worker?> GetWorker(int id)
    {
        var worker = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == id);
        return worker;
    }

    public async Task PostWorker(Worker worker)
    {
        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();
    }
}