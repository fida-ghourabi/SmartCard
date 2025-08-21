using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace STB.SmartCard.Application.MappingImplementation
{
    public class TransactionPendingMapper : ITransactionPendingMapper
    {
        public TransactionPending MapFromDto(CreatePendingTransactionDto dto, string otp)
        {
            var pending = new TransactionPending
            {
                Id = Guid.NewGuid(),
                Type = Enum.Parse<TransactionType>(dto.Type),
                Montant = dto.Montant,
                Date = DateTime.UtcNow,
                CarteId = dto.CarteId,
                OtpCode = otp,
                Expiration = DateTime.UtcNow.AddMinutes(5),
                IsValidated = false
            };

            switch (pending.Type)
            {
                case TransactionType.Paiement:
                    pending.SourcePaiement = dto.SourcePaiement;
                    pending.Lieu = null;
                    pending.NomBanque = null;
                    pending.TypeRetrait = null;
                    pending.CompteDestinataire = null;
                    break;

                case TransactionType.Transfert:
                    pending.CompteDestinataire = dto.CompteDestinataire;
                    pending.Lieu = null;
                    pending.NomBanque = null;
                    pending.TypeRetrait = null;
                    pending.SourcePaiement = null;
                    break;

                case TransactionType.Retrait:
                   
                    pending.Lieu = dto.Lieu;
                    pending.NomBanque = dto.NomBanque;
                    pending.CompteDestinataire = null;
                    pending.SourcePaiement = null;
                    if (!string.IsNullOrEmpty(dto.TypeRetrait) &&
                       Enum.TryParse<RetraitType>(dto.TypeRetrait, out var retraitType))
                    {
                        pending.TypeRetrait = retraitType;
                    }
                    else
                    {
                        pending.TypeRetrait = null; // ou lever une exception si obligatoire
                    }
                    
                    break;
            }

            return pending;
        }
    }
}
