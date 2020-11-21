using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public IConfiguration Configuration { get; }

        public UserController(IConfiguration configuration, IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            Configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult Guest()
        {
            var user = _userService.GenerateAndAddGuestUser();

            var token = _userService.GenerateJwtTokenFor(user.Id);

            var response = _mapper.Map<LoginResponse>(user);
            response.Token = token;

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Google([FromBody] LoginUsingExternalProviderRequest request)
        {
            var user = await _userService.LoginOrCreateUserWithGoogle(request.Token);

            var token = _userService.GenerateJwtTokenFor(user.Id);

            var response = _mapper.Map<LoginResponse>(user);
            response.Token = token;

            return Ok(response);
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
