using System.Runtime.InteropServices.JavaScript;

namespace dotnet_ef_rpg.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // to create unique password hash
    public List<Character>? Characters { get; set; }
}