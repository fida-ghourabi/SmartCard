using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string NumeroCarte { get; set; } 
        public string NomPorteur { get; set; } 
        public string TypeCarte { get; set; } 
        public string EtatCarte { get; set; } 
        public DateTime DateCreation { get; set; }
        public DateTime DateExpiration { get; set; }
        public decimal PlafondRetrait { get; set; }
        public decimal PlafondRetraitMax { get; set; }
        public decimal PlafondPaiement { get; set; }
        public decimal PlafondPaiementMax { get; set; }
        public string ImageUrl { get; set; } 
        public string NumCompte { get; set; } 
        public decimal Solde { get; set; }


        // Infos du client
        public string ClientNom { get; set; }
        public string ClientPrenom { get; set; }
        public string ClientEmail { get; set; }
        public string ClientTelephone { get; set; }
    }
}
