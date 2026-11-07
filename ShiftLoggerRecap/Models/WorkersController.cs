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
}