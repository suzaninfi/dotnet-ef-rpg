﻿namespace dotnet_ef_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<List<Character>> GetAllCharacters();
    Task<Character> GetCharacterById(int id);
    Task<List<Character>> AddCharacter(Character newCharacter);
}