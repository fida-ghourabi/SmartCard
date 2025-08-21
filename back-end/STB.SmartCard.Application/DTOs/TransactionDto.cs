using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } 
        public DateTime Date { get; set; }
        public decimal Montant { get; set; }

        // Champs selon le type
        public string? Lieu { get; set; }
        public string? NomBanque { get; set; }
        public string? TypeRetrait { get; set; }
        public string? SourcePaiement { get; set; }
        public string? CompteDestinataire { get; set; }

        // Infos de la carte liée
        public string NumeroCarte { get; set; } 
        public string TypeCarte { get; set; } 
        public decimal SoldeCompte { get; set; }
    }
}
