using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

[Authorize] // need to be authorized to call these methods
[ApiController] // attribute, indicates that this is used to serve HTTP API responses
[Route("api/[controller]")] // attribute, means this controller can be found at api/Character
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    // inject character service into controller
    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    // [AllowAnonymous] // Don't require authorization for this specific method
    [HttpGet("GetAll")] // Short for [HttpGet] and [Route("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
        return Ok(await _characterService.GetAllCharacters()); // sends status code 200 OK

        // Other options (among others):
        // BadRequest(knight) // 400 status
        // NotFound(knight) // 404 status
    }

    [HttpGet("{id:int}")] // id should match parameter of method
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id)); // returns first where id matches
    }

    // In this post method, the newCharacter object is sent through the body of the request
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(
        UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        if (response.Data is null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }


    [HttpDelete("{id:int}")] // id should match parameter of method
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data is null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}