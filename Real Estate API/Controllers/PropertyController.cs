using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.Property;
using RealEstate.Mediator.Query.Property;

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
        public async Task<IActionResult> CreateProperty([FromForm] CreatePropertyDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyCommand(dto)));
        }

        [HttpGet()]
        public async Task<IEnumerable<PropertyDto>> GetProperties([FromQuery]  GetPropertiesQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
