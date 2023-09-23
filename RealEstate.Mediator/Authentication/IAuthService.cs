using DTOModels;

namespace RealEstate.Mediator.Authentication
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(UserCredentialsDto credentials);
    }
}
