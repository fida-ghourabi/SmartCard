using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.Entities;
using STB.SmartCard.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseImplementation
{
    public class ClientUseCase : IClientUseCase
    {
        private readonly IClientRepository _clientRepository;

        public ClientUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client?> GetClientByUserIdAsync(string userId)
        {
            return await _clientRepository.GetByUserIdAsync(userId);
        }
    }
}
