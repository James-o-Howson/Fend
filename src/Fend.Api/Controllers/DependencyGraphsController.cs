using Fend.Application.Contracts.Dependencies;
using Fend.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;

namespace Fend.Api.Controllers;

public sealed class DependencyGraphsController : ApiControllerBase
{
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DependencyGraphDto>> Scan(ScanCommand query)
    {
        var result = await Mediator.Send(query);
        
        return Ok(result);
    }
}