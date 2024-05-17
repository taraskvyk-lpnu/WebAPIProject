using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }
    
    // POST 
    //api/Auth/Register
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
        {
            // AddRoles to the user
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
            }

            if (identityResult.Succeeded)
            {
                return Ok("User was register, please log in");
            }
        }

        return BadRequest("Error while registering");
    }
    
    
    // POST 
    //api/Auth/Login
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var identityUser = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (identityUser != null)
        {
            var checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);

            if (checkPasswordResult)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);

                if (roles != null)
                {
                    // Create a token
                    var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    return Ok(new LoginResponseDto() { JwtToken = jwtToken });
                }
             
                return BadRequest("Jwt error");   
            }
            else
            {
                return BadRequest("Incorrect password");
            }
        }
        else
        {
            return BadRequest("User with such email does nor exist");    
        }
    }
}