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
    public class CarteRepository : ICarteRepository
    {
        private readonly AppDbContext _context;

        public CarteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Carte>> GetCartesByClientIdAsync(Guid clientId)
        {
            return await _context.Cartes
               .Include(c => c.Compte)
                   .ThenInclude(cpt => cpt.Client)
                         .ThenInclude(client => client.User)
                .Where(c => c.Compte.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Carte?> GetByIdAsync(Guid id)
        {
            return await _context.Cartes
                .Include(c => c.Compte)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Carte carte)
        {
            _context.Cartes.Update(carte);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlafondAsync(Carte carte)
        {
            _context.Cartes.Update(carte);
            await _context.SaveChangesAsync();
        }

        public async Task<Carte?> GetByIdWithCompteAndClientAsync(Guid carteId)
        {
            return await _context.Cartes
                .Include(c => c.Compte)
                    .ThenInclude(cpt => cpt.Client)
                        .ThenInclude(client => client.User)
                .FirstOrDefaultAsync(c => c.Id == carteId);
        }
    }
}
