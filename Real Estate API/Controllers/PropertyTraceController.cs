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

        /// <summary>
        /// Create a new property trace.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to create a new property trace.
        /// </remarks>
        /// <param name="dto">The data required to create a property trace.</param>
        /// <returns>
        /// A response containing the ID of the newly created property trace.
        /// </returns>
        /// <response code="200">Property trace created successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreatePropertyTrace([FromForm] CreatePropertyTraceDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyTraceCommand(dto)));
        }

        /// <summary>
        /// Update an existing property trace.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to update an existing property trace.
        /// </remarks>
        /// <param name="dto">The data required to update a property trace.</param>
        /// <returns>
        /// A response indicating the success of the update operation.
        /// </returns>
        /// <response code="200">Property trace updated successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property trace does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePropertyTrace([FromForm] UpdatePropertyTraceDto dto)
        {
            return await _mediator.Send(new UpdatePropertyTraceCommand { dto = dto });
        }

        /// <summary>
        /// Retrieve property traces by property ID.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" or "Standard" roles to retrieve property traces associated with a specific property.
        /// </remarks>
        /// <param name="propertyId">The unique identifier of the property.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of property traces to include per page.</param>
        /// <returns>
        /// A collection of property traces associated with the specified property.
        /// </returns>
        /// <response code="200">Property traces retrieved successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property or property traces do not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpGet("{propertyId}")]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IEnumerable<PropertyTraceDto>> GetPropertyImagesByPropertyId(Guid propertyId, int page, int pageSize)
        {
            return await _mediator.Send(new GetPropertyTracesByPropertyIdQuery(propertyId, page, pageSize));
        }

        /// <summary>
        /// Delete a property trace by its unique identifier.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to delete a property trace by specifying its unique identifier.
        /// </remarks>
        /// <param name="propertyTraceId">The unique identifier of the property trace to delete.</param>
        /// <returns>
        /// No content response indicating a successful deletion.
        /// </returns>
        /// <response code="204">Property trace deleted successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property trace does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpDelete("{propertyTraceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePropertyImage(Guid propertyTraceId)
        {
            return await _mediator.Send(new DeletePropertyTraceCommand { PropertyTraceId = propertyTraceId });
        }
    }
}
