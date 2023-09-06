namespace dotnet_ef_rpg.Models;

/* The Weapon 'depends' on the Character: it cannot exist without, even though a Character can exist without a weapon.
 *
 */
public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    public Character Character { get; set; }
    // The convention '[ClassName]Id' will tell Entify Framework that this is the corresponding foreign key for the character
    public int CharacterId { get; set; }
}