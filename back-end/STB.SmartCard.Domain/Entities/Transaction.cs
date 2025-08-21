using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.Entities
{
    public enum RetraitType
    {
        DAB,
        GAB,
     
    }
    public enum TransactionType
    {
        Retrait,
        Paiement,
        Transfert
    }
    public class Transaction
    {
        public Guid Id { get; set; }
        public required TransactionType Type { get; set; } // "Retrait", "Paiement", "Transfert"
        public required DateTime Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal Montant { get; set; }
        public string? Lieu { get; set; } 
        public string? NomBanque { get; set; } 
        public RetraitType? TypeRetrait { get; set; } // GAB, Guichet, etc.
        public string? CompteDestinataire { get; set; } 
        public string? SourcePaiement { get; set; }

        // FK
        public Guid CarteId { get; set; }
        public Carte Carte { get; set; }
    }
}
