namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly DataContext _context;

    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);

        _context.Characters
            .Add(character); // no async (there is an async version, we don't use it because we're not making a db query at this point. We just want to start tracking the new character in the edit state.
        await _context.SaveChangesAsync(); // writes the changes to the db, and generates a new id for the character
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync() };
    }


    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var dbCharacters = await _context.Characters.ToListAsync();
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var dbCharacter =
            await _context.Characters.FirstOrDefaultAsync(character =>
                character.Id == id); // returns first where id matches
        return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(dbCharacter) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        try
        {
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

            if (dbCharacter is null)
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
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            if (character is null)
            {
                throw new Exception($"Character with Id '{id}' not found.");
            }

            _context.Characters.Remove(character);

            await _context.SaveChangesAsync();

            return new ServiceResponse<List<GetCharacterDto>>
                { Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync() };
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
}