using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Application.Regions.Queries.GetAll;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/regions")]
    [ApiController]
    public sealed class RegionsController : ControllerBase
    {
        private readonly ISender _sender;

        public RegionsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Gets all regions from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all regions. Returns a list of regions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<RegionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRegions()
        {
            var queryResult = await _sender.Send(new GetAllRegionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }
    }
}
