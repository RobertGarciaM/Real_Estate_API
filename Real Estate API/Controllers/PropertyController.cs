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

        /// <summary>
        /// Create a new property.
        /// </summary>
        /// <remarks>
        /// Allows administrators to create a new property by providing property details in a form.
        /// </remarks>
        /// <param name="dto">The data to create the property with.</param>
        /// <returns>
        /// A response indicating the result of the create operation.
        /// </returns>
        /// <response code="200">Property created successfully.</response>
        /// <response code="400">Bad Request. The provided data is invalid.</response>
        /// <response code="404">Not Found. The specified owner does not exist.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProperty([FromForm] CreatePropertyDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyCommand(dto)));
        }

        /// <summary>
        /// Get a list of properties based on the provided query parameters.
        /// </summary>
        /// <remarks>
        /// Allows users with "Admin" or "Standard" roles to retrieve a list of properties based on various search criteria.
        /// </remarks>
        /// <param name="query">The query parameters to filter and paginate the results.</param>
        /// <returns>
        /// A collection of properties that match the search criteria.
        /// </returns>
        /// <response code="200">Properties retrieved successfully.</response>
        /// <response code="400">Bad Request. The provided query parameters are invalid.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpGet()]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IEnumerable<PropertyDto>> GetProperties([FromQuery]  GetPropertiesQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Update an existing property based on the provided data.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to update an existing property's information.
        /// </remarks>
        /// <param name="dto">The data to update the property with.</param>
        /// <returns>
        /// A response indicating the success of the update operation.
        /// </returns>
        /// <response code="200">Property updated successfully.</response>
        /// <response code="400">Bad Request. The provided data is invalid.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property or owner does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProperty([FromForm] UpdatePropertyDto dto)
        {
            return await _mediator.Send(new UpdatePropertyCommand { UpdateDto = dto });
        }

        /// <summary>
        /// Delete a property based on its unique identifier. Images and traces belonging to the property will also be deleted.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to delete a property by specifying its ID.
        /// </remarks>
        /// <param name="id">The unique identifier of the property to delete.</param>
        /// <returns>
        /// A response indicating the success of the delete operation.
        /// </returns>
        /// <response code="200">Property deleted successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            return await _mediator.Send(new DeletePropertyCommand { PropertyId = id });
        }
    }
}
