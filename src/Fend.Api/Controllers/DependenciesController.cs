using Fend.Abstractions.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Fend.Api.Controllers;

public sealed class DependenciesController : ApiControllerBase
{
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaginatedList<object>>> Search(object query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
}