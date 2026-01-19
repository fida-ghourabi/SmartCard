using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.Services.Chatbot;
using STB.SmartCard.Application.UseCaseInterfaces;
using System.Security.Claims;

namespace STB.SmartCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbot;
        private readonly IClientUseCase _clientUseCase;

        public ChatbotController(IChatbotService chatbot, IClientUseCase clientUseCase)
        {
            _chatbot = chatbot;
            _clientUseCase = clientUseCase;
        }

        [HttpPost("ask")]
        [Authorize]
        public async Task<IActionResult> Ask([FromBody] ChatRequestDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Utilisateur non identifié.");

                var client = await _clientUseCase.GetClientByUserIdAsync(userId);
                if (client == null) return NotFound("Client introuvable");

                var res = await _chatbot.AskAsync(client.Id, dto.Message);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ChatbotController] " + ex);
                return StatusCode(500, new { error = "Erreur interne chatbot: " + ex.Message });
            }
        }
    }
}
