using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

[ApiController] // attribute, indicates that this is used to serve HTTP API responses
[Route("api/[controller]")] // attribute, means this controller can be found at api/Character
public class CharacterController: ControllerBase
{
    private static List<Character> characters = new List<Character> {new Character(), new Character {Name = "Sam", Id = 1}};

    [HttpGet("GetAll")] // Short for [HttpGet] and [Route("GetAll")]
    public ActionResult<List<Character>> Get()
    {
        return Ok(characters); // sends status code 200 OK
        
        // Other options (among other s):
        // BadRequest(knight) // 400 status
        // NotFound(knight) // 404 status
    }
    
    [HttpGet("{id}")] // id should match parameter of method
    public ActionResult<Character> GetSingle(int id)
    {
        return Ok(characters.FirstOrDefault(character => character.Id == id)); // returns first where id matches
        
    }

    // In this post method, the newCharacter object is sent through the body of the request
    [HttpPost]
    public ActionResult<List<Character>> AddCharacter(Character newCharacter)
    {
        characters.Add(newCharacter);
        return Ok(characters);
    }
}