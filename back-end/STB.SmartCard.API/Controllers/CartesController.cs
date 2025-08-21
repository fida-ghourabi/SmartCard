using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.UseCaseImplementation;
using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.Entities;
using System.Security.Claims;

namespace STB.SmartCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class CartesController : ControllerBase
    {
        private readonly ICarteUseCase _carteUseCase;
        private readonly IClientUseCase _clientUseCase;

        public CartesController(ICarteUseCase carteUseCase, IClientUseCase clientUseCase)
        {
            _carteUseCase = carteUseCase;
            _clientUseCase = clientUseCase;
        }


        [HttpGet("mescartes")]
        [Authorize]
        public async Task<IActionResult> GetCartesByClient()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            return Unauthorized("Utilisateur non identifié.");
            var client = await _clientUseCase.GetClientByUserIdAsync(userId);
            if (client == null)
            return NotFound($"Client introuvable pour userId: {userId}");

            var result = await _carteUseCase.GetCartesByClientIdAsync(client.Id);
            return Ok(result);
        }

        [HttpPut("etat")]
        [Authorize]
        public async Task<IActionResult> ModifierEtatCarte([FromBody] CardUpdateEtatDto dto)
        {
            await _carteUseCase.ModifierEtatCarteAsync(dto);
            return Ok(new { message = "État de la carte modifié avec succès." });
        }

        [HttpPut("plafonds")]
        [Authorize]
        public async Task<IActionResult> UpdatePlafonds([FromBody] CardPlafondUpdateDto dto)
        {
            try
            {
                await _carteUseCase.UpdatePlafondsAsync(dto);
                return Ok(new { message = "Plafonds mis à jour avec succès." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> GetCarteStats()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Utilisateur non identifié.");
                var client = await _clientUseCase.GetClientByUserIdAsync(userId);
                if (client == null)
                    return NotFound($"Client introuvable pour userId: {userId}");

                var result = await _carteUseCase.GetCarteStatsByClientIdAsync(client.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        


    }


}
