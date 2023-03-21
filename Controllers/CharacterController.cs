using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

[ApiController] // attribute, indicates that this is used to serve HTTP API responses
[Route("api/[controller]")] // attribute, means this controller can be found at api/Character
public class CharacterController: ControllerBase
{
    private static List<Character> characters = new List<Character> {new Character(), new Character {Name = "Sam"}};

    [HttpGet("GetAll")] // Short for [HttpGet] and [Route("GetAll")]
    public ActionResult<List<Character>> Get()
    {
        return Ok(characters); // sends status code 200 OK
        
        // Other options (among others):
        // BadRequest(knight) // 400 status
        // NotFound(knight) // 404 status
    }
    
    [HttpGet]
    public ActionResult<Character> GetSingle()
    {
        return Ok(characters[0]);
        
    }
}