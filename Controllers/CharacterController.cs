using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

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

    [HttpGet("GetAll")] // Short for [HttpGet] and [Route("GetAll")]
    public async Task<ActionResult<ServiceResponse<List<Character>>>> Get()
    {
        return Ok(await _characterService.GetAllCharacters()); // sends status code 200 OK

        // Other options (among other s):
        // BadRequest(knight) // 400 status
        // NotFound(knight) // 404 status
    }

    [HttpGet("{id:int}")] // id should match parameter of method
    public async Task<ActionResult<ServiceResponse<Character>>> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id)); // returns first where id matches
    }

    // In this post method, the newCharacter object is sent through the body of the request
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(Character newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter));
    }
}