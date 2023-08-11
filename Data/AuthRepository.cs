using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_ef_rpg.Data;

public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public AuthRepository(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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

        return new ServiceResponse<string> { Data = CreateToken(dbUser) };
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

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingsToken is null)
        {
            throw new Exception("AppSettings Token is null");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), Expires = DateTime.Now.AddDays(value: 1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}