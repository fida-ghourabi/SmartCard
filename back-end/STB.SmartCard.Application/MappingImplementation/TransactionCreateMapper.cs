using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.MappingImplementation
{
    public class TransactionCreateMapper : ITransactionCreateMapper
    {
        public Transaction MapToEntity(CreatePendingTransactionDto dto)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Type = Enum.Parse<TransactionType>(dto.Type),
                Date = DateTime.Now,
                Montant = dto.Montant,
                CarteId = dto.CarteId
            };

            switch (transaction.Type)
            {
                case TransactionType.Paiement:
                    transaction.SourcePaiement = dto.SourcePaiement;
                    break;

                case TransactionType.Retrait:
                    transaction.Lieu = dto.Lieu;
                    transaction.NomBanque = dto.NomBanque;

                    if (!string.IsNullOrEmpty(dto.TypeRetrait) &&
                        Enum.TryParse<RetraitType>(dto.TypeRetrait, out var retraitType))
                    {
                        transaction.TypeRetrait = retraitType;
                    }
                    else
                    {
                        transaction.TypeRetrait = null; // ou lever une exception si obligatoire
                    }
                    break;

                case TransactionType.Transfert:
                    transaction.CompteDestinataire = dto.CompteDestinataire;
                    break;
            }


            return transaction;
        }

        // Pour paiement / transfert (création à partir de TransactionPending validé)
        public Transaction MapFromPending(TransactionPending pending)
        {
            if (pending.Type == TransactionType.Retrait)
                throw new ArgumentException("Les transactions de type Retrait ne doivent pas être mappées à partir de TransactionPending.");

            return new Transaction
            {
                Id = Guid.NewGuid(),
                Type = pending.Type,
                Date = DateTime.UtcNow,
                Montant = pending.Montant,
                Lieu = pending.Lieu,
                NomBanque = pending.NomBanque,
                TypeRetrait = pending.TypeRetrait,
                CompteDestinataire = pending.CompteDestinataire,
                SourcePaiement = pending.SourcePaiement,
                CarteId = pending.CarteId
            };
        }

    }
}
