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
    public class CarteMapper : ICarteMapper
    {
        public CardDto ToCardDto(Carte carte)
        {
            return new CardDto
            {
                Id = carte.Id,
                NumeroCarte = carte.NumeroCarte,
                NomPorteur = carte.NomPorteur,
                TypeCarte = carte.TypeCarte,
                EtatCarte = carte.EtatCarte.ToString(),
                DateCreation = carte.DateCreation,
                DateExpiration = carte.DateExpiration,
                PlafondRetrait = carte.PlafondRetrait,
                PlafondRetraitMax = carte.PlafondRetraitMax,
                PlafondPaiement = carte.PlafondPaiement,
                PlafondPaiementMax = carte.PlafondPaiementMax,
                ImageUrl = carte.ImageUrl,
                NumCompte = carte.Compte?.NumCompte ?? "",
                Solde = carte.Compte?.Solde ?? 0,

                  // Infos client
                ClientNom = carte.Compte?.Client?.Nom ?? "",
                ClientPrenom = carte.Compte?.Client?.Prenom ?? "",
                ClientEmail = carte.Compte?.Client?.User?.Email ?? "",
                ClientTelephone = carte.Compte?.Client?.User?.PhoneNumber ?? ""
            };
        }
    }
}
