using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.PropertyImage;
using RealEstate.Mediator.Commands.PropertyImageCommand;
using RealEstate.Mediator.Query.PropertyImages;

namespace Real_Estate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromForm] CreatePropertyImageDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyImageCommand(dto)));
        }

        [HttpGet("{propertyId}")]
        public async Task<IEnumerable<PropertyImageDto>> GetPropertyImagesByPropertyId(Guid propertyId, int page, int pageSize)
        {
            return await _mediator.Send(new GetPropertyImagesByPropertyIdQuery(propertyId, page, pageSize));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOwner([FromForm] UpdatePropertyImagesDto dto)
        {
            return await _mediator.Send(new UpdatePropertyImageCommand { dto = dto });
        }

        [HttpDelete("{propertyImageId}")]
        public async Task<ActionResult> DeletePropertyImage(Guid propertyImageId)
        {
            return await _mediator.Send(new DeletePropertyImageCommand { PropertyImageId = propertyImageId });
        }
    }
}