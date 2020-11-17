using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public IConfiguration Configuration { get; }

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;
            Configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult Guest()
        {
            return Ok(_userService.GenerateAndAddGuestUser().Tokens[0]);
        }
    }
}
