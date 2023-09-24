using DTOModels;
using MediatR;
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

        /// <summary>
        /// Create a new owner.
        /// </summary>
        /// <remarks>
        /// Allows administrators to create a new owner with the provided information.
        /// </remarks>
        /// <param name="dto">Data for creating the owner.</param>
        /// <returns>
        /// A response indicating the result of creating the owner.
        /// </returns>
        /// <response code="200">Owner created successfully.</response>
        /// <response code="400">Bad request. Invalid or missing data in the request or an EntityNullException occurred.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOwner([FromForm] CreateOwnerDto dto)
        {
            return Ok(await _mediator.Send(new CreateOwnerCommand(dto)));
        }

        /// <summary>
        /// Retrieve a paged list of owners.
        /// </summary>
        /// <remarks>
        /// Allows both administrators and standard users to retrieve a paged list of owners.
        /// </remarks>
        /// <param name="page">The page number for pagination (1-based), If the user does not provide it, the first page will return.</param>
        /// <param name="pageSize">The number of owners per page, If the user does not provide it, it will return 10 records</param>
        /// <returns>
        /// A response containing a paged list of owners.
        /// </returns>
        /// <response code="200">Owners retrieved successfully.</response>
        /// <response code="400">Bad request. Invalid page or page size values.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IActionResult> GetPagedOwners(int page, int pageSize)
        {
            IEnumerable<OwnerDto> result = await _mediator.Send(new GetPagedOwnersQuery(page, pageSize));
            return Ok(result);
        }

        /// <summary>
        /// Delete an owner by ID.
        /// </summary>
        /// <remarks>
        /// Allows administrators to delete an owner by providing the owner's unique identifier.
        /// </remarks>
        /// <param name="id">The unique identifier of the owner to delete.</param>
        /// <returns>
        /// A response indicating the result of the delete operation.
        /// </returns>
        /// <response code="200">Owner deleted successfully.</response>
        /// <response code="404">Not Found. The owner with the specified ID was not found.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            return await _mediator.Send(new DeleteOwnerCommand { OwnerId = id });
        }

        /// <summary>
        /// Update an owner's information.
        /// </summary>
        /// <remarks>
        /// Allows administrators to update an owner's information by providing the updated data in a form.
        /// </remarks>
        /// <param name="dto">The data to update the owner with.</param>
        /// <returns>
        /// A response indicating the result of the update operation.
        /// </returns>
        /// <response code="200">Owner updated successfully.</response>
        /// <response code="404">Not Found. The owner with the specified ID was not found.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOwner([FromForm] UpdateOwnerDto dto)
        {
            return await _mediator.Send(new UpdateOwnerCommand { UpdateDto = dto });
        }
    }
}
