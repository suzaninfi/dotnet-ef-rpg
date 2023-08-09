namespace dotnet_ef_rpg.Data;

public interface IAuthRepository
{
    Task<ServiceResponse<int>> Register(User user, string password);

    Task<ServiceResponse<string>> Login(string userName, string password);

    // returning a ServiceResponse would be a bit over the top, since this is only used internally
    Task<bool> UserExists(string userName);
}