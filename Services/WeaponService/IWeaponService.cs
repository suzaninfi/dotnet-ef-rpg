using dotnet_ef_rpg.Dtos.Weapon;

namespace dotnet_ef_rpg.Services.WeaponService;

public interface IWeaponService
{
    Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
}