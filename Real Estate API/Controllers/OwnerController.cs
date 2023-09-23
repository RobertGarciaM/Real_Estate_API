using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Query.Owner;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOwner([FromForm] CreateOwnerDto dto)
        {
            return Ok(await _mediator.Send(new CreateOwnerCommand(dto)));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IActionResult> GetPagedOwners(int page, int pageSize)
        {
            var result = await _mediator.Send(new GetPagedOwnersQuery(page, pageSize));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            return await _mediator.Send(new DeleteOwnerCommand { OwnerId = id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOwner([FromForm] UpdateOwnerDto dto)
        {
            return await _mediator.Send(new UpdateOwnerCommand { UpdateDto = dto });
        }
    }
}
