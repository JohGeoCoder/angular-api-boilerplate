using Models.Logins;
using Models.Registrations;
using System;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResultModel Authenticate(LoginModel login);
        Task<AuthenticationResultModel> Register(RegistrationModel registration);
    }
}
