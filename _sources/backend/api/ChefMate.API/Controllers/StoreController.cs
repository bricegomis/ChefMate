using ChefMate.API.Models.Dto.Store;
using ChefMate.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefMate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StoreController(
    IStoreService storeService,
    IProfileContextService profileContext) : ControllerBase
{
    private readonly IStoreService _storeService = storeService;
    private readonly IProfileContextService _profileService = profileContext;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profileId = _profileService.GetCurrentProfileId(User);
        var stores = await _storeService.GetAllAsync(profileId);
        return Ok(stores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var store = await _storeService.GetByIdAsync(id);
        if (store == null) return NotFound();
        return Ok(store);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] StoreCreateDto dto)
    {
        var profileId = _profileService.GetCurrentProfileId(User);
        var store = await _storeService.AddAsync(dto, profileId);
        return CreatedAtAction(nameof(GetById), new { id = store.Id }, store);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] StoreUpdateDto dto)
    {
        var profileId = _profileService.GetCurrentProfileId(User);
        var store = await _storeService.UpdateAsync(dto, profileId);
        if (store == null) return NotFound();
        return Ok(store);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _storeService.DeleteAsync(id);
        return NoContent();
    }
}