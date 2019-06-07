using Exceptions.Registrations;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Logins;
using Models.Registrations;
using System;
using System.Threading.Tasks;

namespace WebsiteApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel login)
        {
            var loginResult = _authenticationService.Authenticate(login);

            if (loginResult == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registration)
        {
            try
            {
                var registrationResult = await _authenticationService.Register(registration);

                if (registrationResult == null)
                {
                    return BadRequest(new { message = "The user was not registered" });
                }

                return Ok(registrationResult);
            }
            catch (ExceptionUsernameAlreadyExists ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An unexpected error occurred while registering the user." });
            }
        }
    }
}
