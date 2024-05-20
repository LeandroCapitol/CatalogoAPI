using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenereteRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityinMinutes"],
                    out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime =
                    DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName!);

            if (userExists is not null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ResponseDTO { Status = "Error", Message = "User already exists!" });

            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
            };
            
            var result = await _userManager.CreateAsync(user,model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = "User creation failed!" });
            }

            return Ok(new ResponseDTO { Status = "Sucess", Message = "User created sucessfully" });
        }


        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
                return BadRequest("Invalid client request");

            string? acessToken = tokenModel.AcessToken
                                 ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                     ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

            if (principal is null)
                return BadRequest("Invalid acess token/refresh token");

            string userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName!);

            if (user is null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid acess token/refresh token");

            var newAcessToken = _tokenService.GenerateAccessToken(
                                                principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenereteRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
                refreshToken = newRefreshToken,
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null) return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
