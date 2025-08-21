using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.Entities
{
    public enum EtatCarteEnum
    {
        Active,
        Desactive,
        Bloque
    }
    public class Carte
    {
        public Guid Id { get; set; }
        public required string  NumeroCarte { get; set; }
        public required string NomPorteur { get; set; }
        public required string TypeCarte { get; set; } 
        public EtatCarteEnum EtatCarte { get; set; } = EtatCarteEnum.Active;
        public required DateTime DateCreation { get; set; }
        public required DateTime DateExpiration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PlafondRetrait { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal PlafondRetraitMax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PlafondPaiement { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal PlafondPaiementMax { get; set; }
        public  required string ImageUrl { get; set; }

        // FK
        //public Guid ClientId { get; set; }
        //public Client Client { get; set; }

        public Guid CompteId { get; set; }
        public Compte Compte { get; set; }

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


        public Carte() { }
        public Carte(decimal plafondRetraitMax, decimal plafondPaiementMax)
        {
            PlafondRetraitMax = plafondRetraitMax;
            PlafondPaiementMax = plafondPaiementMax;

            PlafondRetrait = plafondRetraitMax;
            PlafondPaiement = plafondPaiementMax;
        }
    }
}
