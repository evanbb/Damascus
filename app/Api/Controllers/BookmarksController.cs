using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Damascus.Core;
using Damascus.Example.Contracts;
using Damascus.Example.Infrastructure;

namespace Damascus.Example.Api
{
    [ApiController]
    [Route("[controller]")]
    public class BookmarksController : ControllerBase
    {
        //private readonly IWidgetCommandRepository _commandRepo;
        //private readonly IWidgetReadRepository _queryRepo;
        public BookmarksController(
            //IWidgetCommandRepository commandRepo,
            //IWidgetReadRepository queryRepo
            )
        {
            //_commandRepo = commandRepo;
            //_queryRepo = queryRepo;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateWidget([FromBody] WidgetData widgetData)
        //{
        //    var newWidget = DomainWidget.CreateNew(
        //        widgetData.Description,
        //        widgetData.Gears.ToDomain()
        //    );

        //    await _commandRepo.CommitAsync(newWidget);

        //    return CreatedAtAction(nameof(GetWidget), new { id = newWidget.Id }, newWidget.ToContract());
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetWidgets()
        //{
        //    return Ok(await _queryRepo.SearchAsync());
        //}

        //[HttpGet("{id:Guid}")]
        //public async Task<IActionResult> GetWidget(Guid id)
        //{
        //    var result = await _queryRepo.FindAsync(id);

        //    return result.HasValue
        //        ? Ok(result.Value)
        //        : NotFound();
        //}

        //[HttpPost("{id:Guid}/description")]
        //public async Task<IActionResult> UpdateDescription(Guid id, [FromBody]string description)
        //{
        //    var result = await _commandRepo.FindAsync(id);

        //    if (!result.HasValue)
        //    {
        //        return NotFound();
        //    }

        //    result.Value.UpdateDescription(description);

        //    await _commandRepo.CommitAsync(result.Value);

        //    return Accepted();
        //}

        //[HttpPost("{id:Guid}/gears/{index:int}")]
        //public async Task<IActionResult> AddGear(Guid id, int index, [FromBody] Gear gear)
        //{
        //    var result = await _commandRepo.FindAsync(id);

        //    if (!result.HasValue)
        //    {
        //        return NotFound();
        //    }

        //    result.Value.AddGearToMotor(gear.ToDomain(), index);

        //    await _commandRepo.CommitAsync(result.Value);

        //    return Accepted();
        //}

        //[HttpPost("{id:Guid}/gears/{index:int}/swaps/{swapWith:int}")]
        //public async Task<IActionResult> SwapGear(Guid id, int index, int swapWith)
        //{
        //    var result = await _commandRepo.FindAsync(id);

        //    if (!result.HasValue)
        //    {
        //        return NotFound();
        //    }

        //    result.Value.SwapGears(index, swapWith);

        //    await _commandRepo.CommitAsync(result.Value);

        //    return Accepted();
        //}
    }
}
