using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.RepositoryInterfaces
{
    public interface ICompteRepository
    {
        Task<Compte?> GetByIdAsync(Guid id);
        Task UpdateAsync(Compte compte);
        Task<List<Compte>> GetComptesByClientIdAsync(Guid clientId);


    }
}
