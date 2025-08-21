using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.RepositoryInterfaces
{
    public interface ITransactionPendingRepository
    {
        Task AddAsync(TransactionPending pending);
        Task<TransactionPending?> GetByIdAsync(Guid id);
        Task UpdatetransAsync(TransactionPending pending);
    }
}
