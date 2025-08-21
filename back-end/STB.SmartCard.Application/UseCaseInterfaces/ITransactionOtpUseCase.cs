using STB.SmartCard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface ITransactionOtpUseCase
    {
        Task<string> CreateTransactionPendingAsync(CreatePendingTransactionDto dto);
        Task ValidateOtpAsync(ValidateOtpDto dto);
    }
}
