using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactionsByClientIdAsync(Guid clientId);
        Task<List<Transaction>> GetTransactionsByCarteIdAsync(Guid carteId);

        Task AddAsync(Transaction transaction);
        Task<Transaction?> GetLastTransactionByClientIdAsync(Guid clientId);

    }
}
