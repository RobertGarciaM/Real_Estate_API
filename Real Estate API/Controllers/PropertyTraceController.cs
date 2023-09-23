using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreatePropertyTrace([FromForm] CreatePropertyTraceDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyTraceCommand(dto)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePropertyTrace([FromForm] UpdatePropertyTraceDto dto)
        {
            return await _mediator.Send(new UpdatePropertyTraceCommand { dto = dto });
        }

        [HttpGet("{propertyId}")]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IEnumerable<PropertyTraceDto>> GetPropertyImagesByPropertyId(Guid propertyId, int page, int pageSize)
        {
            return await _mediator.Send(new GetPropertyTracesByPropertyIdQuery(propertyId, page, pageSize));
        }

        [HttpDelete("{propertyTraceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePropertyImage(Guid propertyTraceId)
        {
            return await _mediator.Send(new DeletePropertyTraceCommand { PropertyTraceId = propertyTraceId });
        }
    }
}
