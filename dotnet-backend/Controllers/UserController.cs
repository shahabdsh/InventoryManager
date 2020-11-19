using System.Linq;
using System.Threading.Tasks;
using InventoryManager.Api.Dtos;
using InventoryManager.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
            var user = _userService.GenerateAndAddGuestUser();

            var token = _userService.GenerateJwtTokenFor(user.Id);

            return Ok(new TokenResponse
            {
                Token = token
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Google([FromBody] LoginUsingExternalProviderRequest request)
        {
            var user = await _userService.LoginOrCreateUserWithGoogle(request.Token);

            var token = _userService.GenerateJwtTokenFor(user.Id);

            return Ok(new TokenResponse
            {
                Token = token
            });
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var header = Request.Headers["Authorization"].First();

            var token = header.Substring("Bearer ".Length).Trim();
            var userId = User.Claims.Single(x => x.Type == nameof(Models.User.Id)).Value;

            _userService.Logout(userId, token);

            return Ok();
        }
    }
}
