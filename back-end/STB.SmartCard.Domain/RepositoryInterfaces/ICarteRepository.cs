using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.RepositoryInterfaces
{
    public interface ICarteRepository
    {
        Task<List<Carte>> GetCartesByClientIdAsync(Guid clientId);
        Task<Carte?> GetByIdAsync(Guid id);
        Task UpdateAsync(Carte carte);
        Task UpdatePlafondAsync(Carte carte);
        public Task<Carte?> GetByIdWithCompteAndClientAsync(Guid carteId);


    }
}
