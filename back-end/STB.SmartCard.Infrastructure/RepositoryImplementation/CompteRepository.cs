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
    public class CompteRepository : ICompteRepository
    {
        private readonly AppDbContext _context;

        public CompteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Compte?> GetByIdAsync(Guid id)
        {
            return await _context.Comptes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Compte compte)
        {
            _context.Comptes.Update(compte);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Compte>> GetComptesByClientIdAsync(Guid clientId)
        {
            return await _context.Comptes
                .Where(c => c.ClientId == clientId)
                .ToListAsync();
        }


    }
}
