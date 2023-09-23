using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Create a new property image.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to create a new image for a property.
        /// </remarks>
        /// <param name="dto">The data required to create a property image.</param>
        /// <returns>
        /// A response indicating the success of the image creation operation along with the newly created image's ID.
        /// </returns>
        /// <response code="200">Property image created successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property for the image does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOwner([FromForm] CreatePropertyImageDto dto)
        {
            return Ok(await _mediator.Send(new CreatePropertyImageCommand(dto)));
        }

        /// <summary>
        /// Retrieve a paginated list of property images for a specific property.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" or "Standard" role to retrieve property images associated with a property.
        /// </remarks>
        /// <param name="propertyId">The unique identifier of the property.</param>
        /// <param name="page">The page number for pagination (1-based), If the user does not provide it, the first page will return.</param>
        /// <param name="pageSize">The maximum number of items to return per page, If the user does not provide it, it will return 10 records</param>
        /// <returns>
        /// A paginated list of property images associated with the specified property.
        /// </returns>
        /// <response code="200">Property images retrieved successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property or property images do not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpGet("{propertyId}")]
        [Authorize(Roles = "Admin,Standard")]
        public async Task<IEnumerable<PropertyImageDto>> GetPropertyImagesByPropertyId(Guid propertyId, int page, int pageSize)
        {
            return await _mediator.Send(new GetPropertyImagesByPropertyIdQuery(propertyId, page, pageSize));
        }

        /// <summary>
        /// Update a property image's information.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to update the details of a property image.
        /// </remarks>
        /// <param name="dto">The data required to update the property image.</param>
        /// <returns>
        /// A response indicating the result of the update operation.
        /// </returns>
        /// <response code="200">Property image information updated successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property image does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOwner([FromForm] UpdatePropertyImagesDto dto)
        {
            return await _mediator.Send(new UpdatePropertyImageCommand { dto = dto });
        }

        /// <summary>
        /// Delete a property image.
        /// </summary>
        /// <remarks>
        /// Allows users with the "Admin" role to delete a property image by its ID.
        /// </remarks>
        /// <param name="propertyImageId">The ID of the property image to delete.</param>
        /// <returns>
        /// A response indicating the result of the delete operation.
        /// </returns>
        /// <response code="200">Property image deleted successfully.</response>
        /// <response code="401">Unauthorized. The user does not have the necessary permissions.</response>
        /// <response code="404">Not Found. The specified property image does not exist.</response>
        /// <response code="500">Internal server error. An unexpected error occurred while processing the request.</response>
        [HttpDelete("{propertyImageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePropertyImage(Guid propertyImageId)
        {
            return await _mediator.Send(new DeletePropertyImageCommand { PropertyImageId = propertyImageId });
        }
    }
}