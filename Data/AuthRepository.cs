using System.Security.Cryptography;
using System.Text;

namespace dotnet_ef_rpg.Data;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;

    public AuthRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        var response = new ServiceResponse<int>();

        if (await UserExists(user.UserName))
        {
            response.Success = false;
            response.Message = "User already exists";
            return response;
        }

        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;


        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> Login(string userName, string password)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(userName.ToLower()));
        if (dbUser is null)
        {
            // Note: be careful what information you return in the message, an attacker could use it!
            return new ServiceResponse<string> { Success = false, Message = "User not found" };
        }

        if (!VerifyPasswordHash(password, dbUser.PasswordHash, dbUser.PasswordSalt))
        {
            return new ServiceResponse<string> { Success = false, Message = "Wrong Password" };
        }

        return new ServiceResponse<string> { Data = dbUser.Id.ToString() };
    }

    public async Task<bool> UserExists(string userName)
    {
        return await _context.Users.AnyAsync(user => user.UserName.ToLower() == userName);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
}