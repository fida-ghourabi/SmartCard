using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.DTOs.Auth;
using STB.SmartCard.Application.UseCaseInterfaces;

namespace STB.SmartCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase;

        public AuthController(IAuthUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var msg = await _authUseCase.RegisterAsync(dto);
            return Ok(new { message = msg });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authUseCase.LoginAsync(dto);
            return Ok(response); // renvoie token + nom + prenom
        }


    }
}
