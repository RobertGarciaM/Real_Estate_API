using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.PropertyImageCommand;
using RealEstate.Mediator.Commands.PropertyTraceCommand;
using RealEstate.Mediator.Query.PropertyImages;
using RealEstate.Mediator.Query.PropertyTrace;

namespace Real_Estate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTraceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyTraceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePropertyTrace([FromForm] CreatePropertyTraceDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyTraceCommand(dto)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePropertyTrace(Guid id, [FromForm] UpdatePropertyTraceDto dto)
        {
            return await _mediator.Send(new UpdatePropertyTraceCommand { PropertyTraceId = id, dto = dto });
        }

        [HttpGet("{propertyId}")]
        public async Task<IEnumerable<PropertyTraceDto>> GetPropertyImagesByPropertyId(Guid propertyId, int page, int pageSize)
        {
            return await _mediator.Send(new GetPropertyTracesByPropertyIdQuery(propertyId, page, pageSize));
        }

        [HttpDelete("{propertyTraceId}")]
        public async Task<ActionResult> DeletePropertyImage(Guid propertyTraceId)
        {
            return await _mediator.Send(new DeletePropertyTraceCommand { PropertyTraceId = propertyTraceId });
        }
    }
}
