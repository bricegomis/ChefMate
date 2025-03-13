using ChefMate.API.Manager;
using ChefMate.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChefMate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(ILogger<ProfileController> logger,
                               IProfileManager profilManager) : ControllerBase
{
    private readonly ILogger<ProfileController> _logger = logger;
    private readonly IProfileManager _profilManager = profilManager;

    [HttpGet(Name = "GetCurrentProfile")]
    [SwaggerOperation(OperationId = "GetCurrentProfile")]
    public ActionResult<Profile> GetCurrentProfile()
    {
        _logger.LogDebug("GetCurrentProfile");

        var profile = _profilManager.CurrentProfile;

        if (profile == null)
        {
            return NotFound($"CurrentProfile not initialized");
        }

        return Ok(profile);
    }
}
