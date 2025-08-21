using STB.SmartCard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface ITransactionUseCase
    {
        Task<List<TransactionDto>> GetAllTransactionsByClientIdAsync(Guid clientId);
        Task<List<TransactionDto>> GetTransactionsByCarteIdAsync(Guid carteId);
        Task CreateTransactionAsync(CreatePendingTransactionDto dto);

        Task<TransactionDto?> GetLastTransactionByClientIdAsync(Guid clientId);

    }
}
