using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DomainWidget = Damascus.Example.Domain.Widget;
using Damascus.Core;
using Damascus.Example.Contracts;
using Damascus.Example.Infrastructure;

namespace Damascus.Example.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WidgetsController : ControllerBase
    {
        private readonly IWidgetCommandRepository _commandRepo;
        private readonly IWidgetReadRepository _queryRepo;
        public WidgetsController(IWidgetCommandRepository commandRepo, IWidgetReadRepository queryRepo)
        {
            _commandRepo = commandRepo;
            _queryRepo = queryRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWidget([FromBody] WidgetData widgetData)
        {
            var newWidget = DomainWidget.CreateNew(
                widgetData.Description,
                widgetData.Gears.ToMotor()
            );

            await _commandRepo.CommitAsync(newWidget);

            return CreatedAtAction(nameof(GetWidget), new { id = newWidget.Id });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetWidget(Guid id)
        {
            var result = await _queryRepo.FindAsync(id);

            if (result.IsNull())
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
