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
    public class TransactionMapper : ITransactionMapper
    {
        public TransactionDto ToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            Type = transaction.Type.ToString(),
            Date = transaction.Date,
            Montant = transaction.Montant,

            // Champs conditionnels
            Lieu = transaction.Type.ToString() == "Retrait" ? transaction.Lieu : null,
            NomBanque = transaction.Type.ToString() == "Retrait" ? transaction.NomBanque : null,
            TypeRetrait = transaction.Type.ToString() == "Retrait" ? transaction.TypeRetrait?.ToString() : null,
            SourcePaiement = transaction.Type.ToString() == "Paiement" ? transaction.SourcePaiement : null,
            CompteDestinataire = transaction.Type.ToString() == "Transfert" ? transaction.CompteDestinataire : null,

            // Carte liée
            NumeroCarte = transaction.Carte.NumeroCarte,
            TypeCarte = transaction.Carte.TypeCarte,
            SoldeCompte = transaction.Carte.Compte.Solde
        };
    }
}
}
