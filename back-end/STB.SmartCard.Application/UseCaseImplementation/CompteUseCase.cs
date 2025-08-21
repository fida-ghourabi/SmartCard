using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseImplementation
{
    public class CompteUseCase : ICompteUseCase
    {
        private readonly ICompteRepository _repository;

        public CompteUseCase(ICompteRepository repository)
        {
            _repository = repository;
        }

        public async Task<decimal> GetSoldeTotalByClientIdAsync(Guid clientId)
        {
            var comptes = await _repository.GetComptesByClientIdAsync(clientId);
            var total = comptes.Sum(c => c.Solde);
            return total;
        }
    }
}
