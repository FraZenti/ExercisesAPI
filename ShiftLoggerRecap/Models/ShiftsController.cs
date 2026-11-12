using Microsoft.AspNetCore.Mvc;

namespace ShiftLoggerRecap;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly ShiftService _service;

    public ShiftsController(ShiftService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ShiftDto>>> GetShifts()
    {
        var shifts = await _service.ShiftsList();
        return shifts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftDto>> GetShift(int id)
    {
        var shift = await _service.GetShift(id);
        if(shift == null)
        {
            return NotFound();
        }
        return shift;
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> PostShift(Shift shift)
    {
        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);
        if (!valid)
        {
            return BadRequest("End time must be after start time.");
        }

        bool workerExists = await _service.WorkerExists(shift.WorkerId);
        if(!workerExists)
        {
            return BadRequest("Worker does not exist.");
        }
        await _service.PostShift(shift);
        return CreatedAtAction(nameof(GetShift), new {id = shift.Id}, shift);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutShift(int id, Shift shift)
    {
        if(id != shift.Id)
        {
            return BadRequest("ID mismatch.");
        }

        bool exists = await _service.ShiftExists(id);
        if(!exists)
        {
            return NotFound();
        }

        bool valid = _service.ValidateShift(shift.StartTime, shift.EndTime);

        if (!valid)
        {
            return BadRequest("End time must be after start time.");
        }

        bool workerExists = await _service.WorkerExists(shift.WorkerId);
        if(!workerExists)
        {
            return BadRequest("Worker does not exist.");
        }

        await _service.PutShift(shift);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        bool deleted = await _service.DeleteShift(id);
        if(!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}