using NorthwindCore.Models;

namespace NorthwindCore.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Logins Authenticate(string userName, string password);
    }
}
