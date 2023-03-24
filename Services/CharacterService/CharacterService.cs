namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static readonly List<Character> characters = new()
        { new Character(), new Character { Name = "Sam", Id = 1 } };

    public async Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)
    {
        characters.Add(newCharacter);
        return new ServiceResponse<List<Character>> { Data = characters };
    }

    public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
    {
        return new ServiceResponse<List<Character>> { Data = characters };
    }

    public async Task<ServiceResponse<Character>> GetCharacterById(int id)
    {
        var character = characters.FirstOrDefault(character => character.Id == id); // returns first where id matches
        return new ServiceResponse<Character> { Data = character };
    }
}