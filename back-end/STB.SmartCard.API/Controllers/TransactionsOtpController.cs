using Microsoft.AspNetCore.Mvc;
using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.UseCaseInterfaces;

namespace STB.SmartCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsOtpController : ControllerBase
    {
        private readonly ITransactionOtpUseCase _otpUseCase;

        public TransactionsOtpController(ITransactionOtpUseCase otpUseCase)
        {
            _otpUseCase = otpUseCase;
        }

        [HttpPost("create-pending")]
        public async Task<IActionResult> CreatePending([FromBody] CreatePendingTransactionDto dto)
        {
            

            var transactionId = await _otpUseCase.CreateTransactionPendingAsync(dto);
            return Ok(new { transactionId});
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpDto dto)
        {
            await _otpUseCase.ValidateOtpAsync(dto);
            return Ok(new { message = "Transaction validée avec succès." });
        }
    }
}
