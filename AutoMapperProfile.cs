using dotnet_ef_rpg.Dtos.Skill;
using dotnet_ef_rpg.Dtos.Weapon;

namespace dotnet_ef_rpg;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<Skill, GetSkillDto>();
    }
}