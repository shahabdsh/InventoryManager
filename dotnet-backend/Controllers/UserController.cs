using InventoryManager.Api.Dtos;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
            var token = _userService.GenerateAndAddGuestUser().Tokens[0];

            return Ok(new TokenResponse
            {
                Token = token
            });
        }
    }
}
