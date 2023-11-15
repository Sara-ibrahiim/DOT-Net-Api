using ApiDemo.HandelResponse;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Services.RegisterServices.Dto;
using Services.Services.TokenServices;
using Services.Services.UserService;
using System.Security.Claims;

namespace ApiDemo.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(
       
           IUserService userService,
           UserManager<AppUser> userManager
            ) {
    
           _userService = userService;
           _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.Register(registerDto);

            if (user == null)
                return BadRequest(new ApiException(400, "Email Already Exit"));


            return Ok(registerDto);



        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto  loginDto)
        {

            var user = await _userService.Login(loginDto);

            if (user == null)
                return Unauthorized(new ApiResponse(401, "Email Already Exit"));

            return Ok(user);

        }
        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
            };
        }

    }
}
