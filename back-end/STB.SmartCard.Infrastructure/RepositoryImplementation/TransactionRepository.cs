using Microsoft.EntityFrameworkCore;
using STB.SmartCard.Domain.Entities;
using STB.SmartCard.Domain.RepositoryInterfaces;
using STB.SmartCard.Infrastructure.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Infrastructure.RepositoryImplementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactionsByClientIdAsync(Guid clientId)
        {
            return await _context.Transactions
                .Include(t => t.Carte)
                    .ThenInclude(c => c.Compte)
                .Where(t => t.Carte.Compte.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByCarteIdAsync(Guid carteId)
        {
            return await _context.Transactions
                .Include(t => t.Carte)
                    .ThenInclude(c => c.Compte)
                .Where(t => t.CarteId == carteId)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction?> GetLastTransactionByClientIdAsync(Guid clientId)
        {
            return await _context.Transactions
                .Include(t => t.Carte)
                .ThenInclude(c => c.Compte)
                .Where(t => t.Carte.Compte.ClientId == clientId)
                .OrderByDescending(t => t.Date)
                .FirstOrDefaultAsync();
        }
    }
}

