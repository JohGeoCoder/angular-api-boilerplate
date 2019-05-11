using ConfigSettings;
using Exceptions.Registrations;
using HelperClasses;
using Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Entities.DbModels;
using Models.Logins;
using Models.Registrations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    //Reference from: http://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api#user-cs
    public class AuthenticationService : IAuthenticationService
    {
        private IUserService<User> _userService;
        private AuthSettings _authSettings;

        public AuthenticationService(
            IUserService<User> userService,
            IOptions<AuthSettings> authSettings)
        {
            _userService = userService;
            _authSettings = authSettings.Value;
        }

        public AuthenticationResultModel Authenticate(LoginModel login)
        {
            var user = _userService.GetAllThenInclude(
                u => u.Username.ToLower() == login.Username.ToLower(),
                IncludeBuilder<User>.Include(u => u.UserRole).ThenInclude(userRole => userRole.Role).Done()
            )
            .SingleOrDefault();

            //Return null if user not found
            if (user == null) return null;

            //Return null if the user doesn't have a password
            if (string.IsNullOrWhiteSpace(user.PasswordHash)) return null;

            //Return null if the password is invalid
            if (!PasswordHelper.ValidatePassword(login.Password, user.PasswordHash)) return null;

            //Authentication successful so generate jwt token
            var token = GenerateToken(user);

            var authenticationResultModel = new AuthenticationResultModel
            {
                UserId = user.Id,
                Token = token
            };

            return authenticationResultModel;
        }

        public async Task<AuthenticationResultModel> Register(RegistrationModel registration)
        {
            //Check if username already exists.
            var existingUser = _userService.GetAll(u => u.Username == registration.Username).FirstOrDefault();

            if (existingUser != null) throw new ExceptionUsernameAlreadyExists();

            var passwordHash = PasswordHelper.HashPassword(registration.Password);

            var userToCreate = new User
            {
                FirstName = registration.FirstName.ToLower(),
                LastName = registration.LastName.ToLower(),
                Email = registration.Email.ToLower(),
                Username = registration.Username.ToLower(),
                PasswordHash = passwordHash
            };

            //userToCreate.UserRole.Add(new UserRole
            //{
            //    RoleId = (long)Roles.BasicUser
            //});

            await _userService.Create(userToCreate);

            var createdUser = _userService.GetAllThenInclude(
                u => u.Id == userToCreate.Id,
                IncludeBuilder<User>.Include(u => u.UserRole).ThenInclude(ur => ur.Role).Done()
            ).Single();

            //Registration successful so generate jwt token
            var token = GenerateToken(createdUser);

            var authenticationResultModel = new AuthenticationResultModel
            {
                UserId = createdUser.Id,
                Token = token
            };

            return authenticationResultModel;
        }

        private string GenerateToken(User user)
        {
            //Retrieve the list of Roles for the User as Claim objects.
            var roleClaims = user.UserRole.Select(role =>
                new Claim(ClaimTypes.Role, role.Role.Name)
            );

            var userIdClaim = new Claim(ClaimTypes.Name, user.Id.ToString());

            //Condense all the different types of Claims into one collection.
            var claims = new List<Claim>();
            claims.AddRange(roleClaims);
            claims.Add(userIdClaim);

            //Generate the token
            var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
