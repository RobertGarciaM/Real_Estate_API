using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Query.PropertyCommand;

namespace Real_Estate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController: ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProperty([FromForm] CreatePropertyDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyCommand(dto)));
        }

        [HttpGet()]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IEnumerable<PropertyDto>> GetProperties([FromQuery]  GetPropertiesQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProperty([FromForm] UpdatePropertyDto dto)
        {
            return await _mediator.Send(new UpdatePropertyCommand { UpdateDto = dto });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            return await _mediator.Send(new DeletePropertyCommand { PropertyId = id });
        }
    }
}
