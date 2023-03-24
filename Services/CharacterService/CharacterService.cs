namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static readonly List<Character> characters = new()
        { new Character(), new Character { Name = "Sam", Id = 1 } };

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        characters.Add(newCharacter);
        return new ServiceResponse<List<GetCharacterDto>> { Data = characters };
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        return new ServiceResponse<List<GetCharacterDto>> { Data = characters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var character = characters.FirstOrDefault(character => character.Id == id); // returns first where id matches
        return new ServiceResponse<Character> { Data = character };
    }
}