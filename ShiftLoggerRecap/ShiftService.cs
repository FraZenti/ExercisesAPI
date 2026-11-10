using Microsoft.EntityFrameworkCore;

namespace ShiftLoggerRecap;

public class ShiftService
{
    private readonly ShiftLoggerContext _context;

    public ShiftService(ShiftLoggerContext context)
    {
        _context = context;
    }

    public async Task<List<Worker>> WorkersList()
    {
        var workers = await _context.Workers
            .Include(w => w.Shifts)
            .ToListAsync();
        return workers;
    }

    public async Task<List<Shift>> ShiftsList()
    {
        var shifts = await _context.Shifts
            //.Include(s => s.Worker)
            .ToListAsync();

        return shifts;
    }

    public async Task<Worker?> GetWorker(int id)
    {
        var worker = await _context.Workers
            .FirstOrDefaultAsync(w => w.Id == id);
        return worker;
    }

    public async Task<Shift?> GetShift(int id)
    {
        var shift = await _context.Shifts
            .FirstOrDefaultAsync(s => s.Id == id);
        return shift;
    }

    public async Task<List<Shift>?> GetWorkerShifts(int workerId)
    {
        if(!await WorkerExists(workerId))
        {
            return null;
        }

        var shifts = await _context.Shifts
            .Where(s => s.WorkerId == workerId)
            .ToListAsync();
        return shifts;
    }

    public async Task PostWorker(Worker worker)
    {
        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();
    }

    public async Task PostShift(Shift shift)
    {
        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
    }

    public async Task PutWorker(Worker worker)
    {
        _context.Entry(worker).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task PutShift(Shift shift)
    {
        _context.Entry(shift).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> WorkerExists(int id)
    {
        return await _context.Workers.AnyAsync(w => w.Id == id);
    }

    public async Task<bool> ShiftExists(int id)
    {
        return await _context.Shifts.AnyAsync(s => s.Id == id);
    }

    public bool ValidateShift(DateTime start, DateTime end)
    {
        if (end <= start)
            return false;

        return true;
    }
    public async Task<bool> DeleteWorker(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker == null)
            return false;

        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift == null)
            return false;

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }
}