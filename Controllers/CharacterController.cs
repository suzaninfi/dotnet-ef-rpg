using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

[ApiController] // attribute, indicates that this is used to serve HTTP API responses
[Route("api/[controller]")] // attribute, means this controller can be found at api/Character
public class CharacterController: ControllerBase
{
    private static Character knight = new Character();

    [HttpGet]
    public ActionResult<Character> Get()
    {
        return Ok(knight); // sends status code 200 OK
        
        // Other options (among others):
        // BadRequest(knight) // 400 status
        // NotFound(knight) // 404 status
    }
}