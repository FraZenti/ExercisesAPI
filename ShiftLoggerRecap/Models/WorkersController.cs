using Microsoft.AspNetCore.Mvc;

namespace ShiftLoggerRecap;

[ApiController]
[Route("api/[controller]")]
public class WorkersController : ControllerBase
{
    private readonly ShiftService _service;

    public WorkersController(ShiftService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Worker>>> GetWorkers()
    {
        var workers = await _service.WorkersList();
        return workers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Worker>> GetWorker(int id)
    {
        var worker = await _service.GetWorker(id);
        if(worker == null)
        {
            return NotFound();
        }
        return worker;
    }

    [HttpPost]
    public async Task<ActionResult<Worker>> PostWorker(Worker worker)
    {
        await _service.PostWorker(worker);
        return CreatedAtAction(nameof(GetWorker), new {id = worker.Id}, worker);
    }
}