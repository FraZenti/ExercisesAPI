using Microsoft.EntityFrameworkCore;

namespace ShiftLoggerRecap;

public class ShiftService
{
    private readonly ShiftLoggerContext _context;

    public ShiftService(ShiftLoggerContext context)
    {
        _context = context;
    }

    public async Task<List<WorkerDto>> WorkersList()
    {
        var workers = await _context.Workers
            .Select(
                w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Shifts = w.Shifts
                    .Select(
                        s => new WorkerShiftDto
                        {
                            Id = w.Id,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime
                        }
                    ).ToList()
                }
            )
            .ToListAsync();
        return workers;
    }

    public async Task<List<ShiftDto>> ShiftsList()
    {
        var shifts = await _context.Shifts
            .Select(
                s => new ShiftDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    WorkerId = s.WorkerId,
                    WorkerName = s.Worker!.Name
                }
            )
            .ToListAsync();

        return shifts;
    }

    public async Task<WorkerDto?> GetWorker(int id)
    {
        var worker = await _context.Workers
            .Where(w => w.Id == id)
            .Select(
                w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Shifts = w.Shifts
                    .Select(
                        s => new WorkerShiftDto
                        {
                            Id = s.Id,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime
                        }
                    ).ToList()
                }
            )
            .FirstOrDefaultAsync();
        return worker;
    }

    public async Task<ShiftDto?> GetShift(int id)
    {
        var shift = await _context.Shifts
            .Where((s => s.Id == id))
            .Select(
                s => new ShiftDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    WorkerId = s.WorkerId,
                    WorkerName = s.Worker!.Name
                }
            )
            .FirstOrDefaultAsync();
        return shift;
    }

    public async Task<List<WorkerShiftDto>?> GetWorkerShifts(int workerId)
    {
        if (!await WorkerExists(workerId))
        {
            return null;
        }

        var shifts = await _context.Shifts
            .Where(s => s.WorkerId == workerId)
            .Select(
                s => new WorkerShiftDto
                {
                    Id = s.Id,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                }
            )
            .ToListAsync();
        return shifts;
    }

    public async Task PostWorker(WorkerDto workerDto)
    {
        Worker worker = new Worker {
            Name = workerDto.Name
        };
        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();
        workerDto.Id = worker.Id;
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