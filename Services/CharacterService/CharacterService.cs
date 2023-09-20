using System.Security.Claims;

namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());

        _context.Characters
            .Add(character); // no async (there is an async version, we don't use it because we're not making a db query at this point. We just want to start tracking the new character in the edit state.
        await _context.SaveChangesAsync(); // writes the changes to the db, and generates a new id for the character
        return new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync()
        };
    }


    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        // only get characters from the DB that are related to the given user
        var dbCharacters =
            await _context.Characters
                .Include(character => character.Weapon)
                .Include(character => character.Skills)
                .Where(character => character.User!.Id == GetUserId()).ToListAsync();
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var dbCharacter =
            await _context.Characters
                .Include(character => character.Weapon)
                .Include(character => character.Skills)
                .FirstOrDefaultAsync(character =>
                    character.Id == id &&
                    character.User!.Id ==
                    GetUserId()); // returns first where id matches and user matches logged in user
        return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(dbCharacter) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        try
        {
            var dbCharacter = await _context.Characters
                .Include(c =>
                    c.User) // without this, EF does not include the related object to the character in this case
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

            if (dbCharacter is null || dbCharacter.User!.Id != GetUserId())
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
            }

            dbCharacter.Name = updatedCharacter.Name;
            dbCharacter.HitPoints = updatedCharacter.HitPoints;
            dbCharacter.Strength = updatedCharacter.Strength;
            dbCharacter.Defense = updatedCharacter.Defense;
            dbCharacter.Intelligence = updatedCharacter.Intelligence;
            dbCharacter.Class = updatedCharacter.Class;

            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(dbCharacter) };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<GetCharacterDto>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

            if (character is null)
            {
                throw new Exception($"Character with Id '{id}' not found.");
            }

            _context.Characters.Remove(character);

            await _context.SaveChangesAsync();

            return new ServiceResponse<List<GetCharacterDto>>
            {
                Data = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync()
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<List<GetCharacterDto>>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .Include(character => character.Weapon)
                .Include(character =>
                    character.Skills) // If Skills had additional data, like lists of side effects, we could also include those by adding .thenInclude(...)
                .FirstOrDefaultAsync(character =>
                    character.Id == newCharacterSkill.CharacterId && character.User!.Id == GetUserId());
            if (character is null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            var skill = await _context.Skills.FirstOrDefaultAsync(skill => skill.Id == newCharacterSkill.SkillId);

            if (skill is null)
            {
                response.Success = false;
                response.Message = "Skill not found";
                return response;
            }

            character.Skills!.Add(skill);
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


    // get id of the logged in user
    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}