using STB.SmartCard.Domain.Entities;
using STB.SmartCard.Domain.RepositoryInterfaces;
using STB.SmartCard.Infrastructure.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace STB.SmartCard.Infrastructure.RepositoryImplementation
{
    
    
        public class TransactionPendingRepository : ITransactionPendingRepository
        {
            private readonly AppDbContext _context;

            public TransactionPendingRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(TransactionPending pending)
            {
                await _context.TransactionPendings.AddAsync(pending);
                await _context.SaveChangesAsync();
            }

            public async Task<TransactionPending?> GetByIdAsync(Guid id)
            {
                return await _context.TransactionPendings
                                     .Include(t => t.Carte)
                                     .ThenInclude(c => c.Compte)
                                     .FirstOrDefaultAsync(t => t.Id == id);
            }

            public async Task UpdatetransAsync(TransactionPending pending)
            {
                _context.TransactionPendings.Update(pending);
                await _context.SaveChangesAsync();
            }


    }
    
}
