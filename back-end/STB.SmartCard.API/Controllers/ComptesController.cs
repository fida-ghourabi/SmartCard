using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.UseCaseImplementation;
using STB.SmartCard.Application.UseCaseInterfaces;
using System.Security.Claims;

namespace STB.SmartCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComptesController : ControllerBase
    {
        private readonly ICompteUseCase _compteUseCase;
        private readonly IClientUseCase _clientUseCase;

        public ComptesController(ICompteUseCase compteUseCase, IClientUseCase clientUseCase)
        {
            _compteUseCase = compteUseCase;
            _clientUseCase = clientUseCase;
        }

        [HttpGet("solde-total")]
        public async Task<IActionResult> GetSoldeTotal()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Utilisateur non identifié.");
                var client = await _clientUseCase.GetClientByUserIdAsync(userId);
                if (client == null)
                    return NotFound($"Client introuvable pour userId: {userId}");

                var result = await _compteUseCase.GetSoldeTotalByClientIdAsync(client.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
