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

    public async Task PutWorker(Worker worker)
    {
        _context.Entry(worker).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> WorkerExists(int id)
    {
        return await _context.Workers.AnyAsync(w => w.Id == id);
    }
    public async Task<bool> DeleteWorker(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if(worker == null)
            return false;
        
        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();

        return true;
    }
}