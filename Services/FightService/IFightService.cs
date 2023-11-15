using dotnet_ef_rpg.Dtos.Fight;

namespace dotnet_ef_rpg.Services.FightService;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
}