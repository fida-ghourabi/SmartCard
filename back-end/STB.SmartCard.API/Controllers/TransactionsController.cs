using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.UseCaseInterfaces;
using System.Security.Claims;

namespace STB.SmartCard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;
        private readonly IClientUseCase _clientUseCase;

        public TransactionsController(ITransactionUseCase transactionUseCase, IClientUseCase clientUseCase)
        {
            _transactionUseCase = transactionUseCase;
            _clientUseCase = clientUseCase;
        }

        [HttpGet("client")]
        public async Task<IActionResult> GetTransactionsByClientId()
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilisateur non identifié.");
            var client = await _clientUseCase.GetClientByUserIdAsync(userId);
            if (client == null)
                return NotFound($"Client introuvable pour userId: {userId}");

            var result = await _transactionUseCase.GetAllTransactionsByClientIdAsync(client.Id);
            return Ok(result);
        }

        [HttpGet("carte/{carteId}")]
        public async Task<IActionResult> GetTransactionsByCarteId(Guid carteId)
        {
            var result = await _transactionUseCase.GetTransactionsByCarteIdAsync(carteId);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreatePendingTransactionDto dto)
        {
            await _transactionUseCase.CreateTransactionAsync(dto);
            return Ok(new { message = "Transaction créée avec succès" });
        }

        [HttpGet("dernier")]
        public async Task<IActionResult> GetLastTransaction()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilisateur non identifié.");
            var client = await _clientUseCase.GetClientByUserIdAsync(userId);
            if (client == null)
                return NotFound($"Client introuvable pour userId: {userId}");

            var transaction = await _transactionUseCase.GetLastTransactionByClientIdAsync(client.Id);
            if (transaction == null)
                return NotFound("Aucune transaction trouvée pour ce client.");

            return Ok(transaction);
        }
    }
}
