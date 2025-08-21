using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.MappingImplementation;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseImplementation
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly ITransactionRepository _repository;
        private readonly ITransactionMapper _mapper;
        private readonly ITransactionCreateMapper _CreateMapper;
        private readonly ICarteRepository _carteRepository;
        private readonly ICompteRepository _compteRepository;




        public TransactionUseCase(ITransactionRepository repository, ITransactionMapper mapper, ITransactionCreateMapper CreateMapper, ICarteRepository carteRepository,ICompteRepository compteRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _CreateMapper = CreateMapper;
            _carteRepository = carteRepository;
            _repository = repository;
            _compteRepository = compteRepository;
        }

        public async Task<List<TransactionDto>> GetAllTransactionsByClientIdAsync(Guid clientId)
        {
            var transactions = await _repository.GetAllTransactionsByClientIdAsync(clientId);
            return transactions.Select(t => _mapper.ToDto(t)).ToList();
        }

        public async Task<List<TransactionDto>> GetTransactionsByCarteIdAsync(Guid carteId)
        {
            var transactions = await _repository.GetTransactionsByCarteIdAsync(carteId);
            return transactions.Select(t => _mapper.ToDto(t)).ToList();
        }

        public async Task CreateTransactionAsync(CreatePendingTransactionDto dto)
        {
            var carte = await _carteRepository.GetByIdAsync(dto.CarteId);
            if (carte == null) throw new Exception("Carte introuvable");

            var compte = await _compteRepository.GetByIdAsync(carte.CompteId);
            if (compte == null) throw new Exception("Compte introuvable");

            if (compte.Solde < dto.Montant)
                throw new Exception("Solde insuffisant");

            // Mise à jour du solde
            switch (dto.Type.ToLower())
            {
                case "paiement":
                case "retrait":
                case "transfert":
                    compte.Solde -= dto.Montant;
                    break;
                default:
                    throw new Exception("Type de transaction invalide");
            }

            var transaction = _CreateMapper.MapToEntity(dto);
            await _repository.AddAsync(transaction);
            await _compteRepository.UpdateAsync(compte);
        }

        public async Task<TransactionDto?> GetLastTransactionByClientIdAsync(Guid clientId)
        {
            var transaction = await _repository.GetLastTransactionByClientIdAsync(clientId);
            if (transaction == null) return null;
            return _mapper.ToDto(transaction);
        }

    }
}
