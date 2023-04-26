namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static readonly List<Character> characters = new()
        { new Character(), new Character { Name = "Sam", Id = 1 } };

    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.Id = characters.Max(c => c.Id) + 1;
        characters.Add(character);
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
    }


    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var character = characters.FirstOrDefault(character => character.Id == id); // returns first where id matches
        return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(character) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        try
        {
            var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

            if (character is null)
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
            }

            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;

            return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(character) };
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
            var character = characters.FirstOrDefault(c => c.Id == id);

            if (character is null)
            {
                throw new Exception($"Character with Id '{id}' not found.");
            }

            characters.Remove(character);

            return new ServiceResponse<List<GetCharacterDto>>
                { Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
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