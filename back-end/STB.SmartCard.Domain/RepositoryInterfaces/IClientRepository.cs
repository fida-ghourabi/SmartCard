using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.RepositoryInterfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByUserIdAsync(string userId);
        Task AddAsync(Client client);
    }
}
