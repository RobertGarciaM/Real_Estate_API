using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.QueryHandlers.Owner;

namespace Real_Estate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromForm] CreateOwnerDto dto)
        {
            return Ok(await _mediator.Send(new CreateOwnerCommand(dto)));
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedOwners(int page, int pageSize)
        {
            var result = await _mediator.Send(new GetPagedOwnersCommand { Page = page, PageSize = pageSize });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            return await _mediator.Send(new DeleteOwnerCommand { OwnerId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromForm] UpdateOwnerDto dto)
        {
            return await _mediator.Send(new UpdateOwnerCommand { OwnerId = id, UpdateDto = dto });
        }
    }
}
