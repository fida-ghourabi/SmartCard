using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.MappingImplementation;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Application.Services.Sms;
using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.Entities;
using STB.SmartCard.Domain.RepositoryInterfaces;

using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace STB.SmartCard.Application.UseCaseImplementation
{
    public class TransactionOtpUseCase : ITransactionOtpUseCase
    {
        private readonly ITransactionPendingRepository _pendingRepo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly ICarteRepository _carteRepo;
        private readonly ICompteRepository _compteRepo;
        private readonly ITransactionPendingMapper _transactionPendingMapper;
        private readonly ITransactionCreateMapper _CreateMapper;

        //private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;


        public TransactionOtpUseCase(
            ITransactionPendingRepository pendingRepo,
            ITransactionRepository transactionRepo,
            ICarteRepository carteRepo,
            ICompteRepository compteRepo,
            //IEmailService emailService,
             ISmsService smsService,
             ITransactionCreateMapper CreateMapper,
             ITransactionPendingMapper transactionPendingMapper

)
        {
            _pendingRepo = pendingRepo;
            _transactionRepo = transactionRepo;
            _carteRepo = carteRepo;
            _compteRepo = compteRepo;
            //_emailService = emailService;
            _smsService = smsService;
            _CreateMapper = CreateMapper;
            _transactionPendingMapper = transactionPendingMapper;



        }

        public async Task<string> CreateTransactionPendingAsync(CreatePendingTransactionDto dto)
        {
            if (dto.Type == TransactionType.Retrait.ToString())
            {
                var carte = await _carteRepo.GetByIdWithCompteAndClientAsync(dto.CarteId);
                if (carte?.Compte == null)
                    throw new Exception("Carte ou compte introuvable.");

                if (carte.Compte.Solde < dto.Montant)
                    throw new Exception("Solde insuffisant.");

                var transaction = _CreateMapper.MapToEntity(dto);

                carte.Compte.Solde -= dto.Montant;

                await _transactionRepo.AddAsync(transaction);
                await _compteRepo.UpdateAsync(carte.Compte);

                return transaction.Id.ToString();
            }
            else if (dto.Type == TransactionType.Paiement.ToString() || dto.Type == TransactionType.Transfert.ToString())
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var pending = _transactionPendingMapper.MapFromDto(dto, otp);

                await _pendingRepo.AddAsync(pending);

                var carte = await _carteRepo.GetByIdWithCompteAndClientAsync(dto.CarteId);
                if (carte?.Compte?.Client?.User == null)
                    throw new Exception("Client ou e-mail introuvable.");

                var mobile = carte.Compte.Client.User.PhoneNumber;
                await _smsService.SendOtpSmsAsync(mobile, otp);

                return pending.Id.ToString();
            }
            else
            {
                throw new Exception("Type de transaction non supporté.");
            }
        }

        public async Task ValidateOtpAsync(ValidateOtpDto dto)
        {
            var pending = await _pendingRepo.GetByIdAsync(dto.TransactionPendingId);
            if (pending == null)
                throw new Exception("Transaction en attente introuvable.");

            if (pending.Expiration < DateTime.UtcNow)
                throw new Exception("Le code OTP a expiré.");

            if (pending.IsValidated)
                throw new Exception("Cette transaction a déjà été validée.");

            if (pending.OtpCode != dto.OtpCode)
                throw new Exception("Code OTP incorrect.");

            // Marquer comme validée
            pending.IsValidated = true;

            var carte = await _carteRepo.GetByIdAsync(pending.CarteId);
            if (carte == null) throw new Exception("Carte introuvable.");

            var compte = await _compteRepo.GetByIdAsync(carte.CompteId);
            if (compte == null) throw new Exception("Compte introuvable.");

            if (compte.Solde < pending.Montant)
                throw new Exception("Solde insuffisant.");

            // Mise à jour du solde
            compte.Solde -= pending.Montant;
            
            var transaction = _CreateMapper.MapFromPending(pending);
            
            

            // Enregistrement
            await _transactionRepo.AddAsync(transaction);
            await _pendingRepo.UpdatetransAsync(pending);
            await _compteRepo.UpdateAsync(compte);
        }
    }
}
