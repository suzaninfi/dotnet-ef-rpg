﻿namespace dotnet_ef_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static readonly List<Character> characters = new()
        { new Character(), new Character { Name = "Sam", Id = 1 } };

    public async Task<List<Character>> AddCharacter(Character newCharacter)
    {
        characters.Add(newCharacter);
        return characters;
    }

    public async Task<List<Character>> GetAllCharacters()
    {
        return characters;
    }

    public async Task<Character> GetCharacterById(int id)
    {
        var character = characters.FirstOrDefault(character => character.Id == id); // returns first where id matches
        if (character is not null)
        {
            return character;
        }

        throw new Exception("Character not found");
    }
}