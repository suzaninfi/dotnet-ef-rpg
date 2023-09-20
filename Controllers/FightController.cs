using Microsoft.AspNetCore.Mvc;

namespace dotnet_ef_rpg.Controllers;

[ApiController]
[Route("[controller]")]
// Would probably make sense to add authentication here (only let characters fight of logged in users), but was left out of the course
public class FightController : ControllerBase

{
    private readonly IFightService _fightService;

    public FightController(IFightService fightService)
    {
        _fightService = fightService;
    }
}