using System.Security.Claims;
using dotnet_ef_rpg.Dtos.Weapon;

namespace dotnet_ef_rpg.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
        var response = new ServiceResponse<GetCharacterDto>();

        try
        {
            // Get character with the given id, that also actually belongs to the logged in user
            var character = await _context.Characters.FirstOrDefaultAsync(character =>
                character.Id == newWeapon.CharacterId && character.User!.Id ==
                int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (character is null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            var weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage = newWeapon.Damage,
                Character = character
            };

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception exception)
        {
            response.Success = false;
            response.Message = exception.Message;
        }

        return response;
    }
}