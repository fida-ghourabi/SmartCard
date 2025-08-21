using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class TransactionDtoToCreate
    {
        [Required]
        public string Type { get; set; }  

        [Required]
        public decimal Montant { get; set; }

        [Required]
        public Guid CarteId { get; set; }

        // Pour Paiement
        public string? SourcePaiement { get; set; }

        // Pour Retrait
        public string? Lieu { get; set; }
        public string? NomBanque { get; set; }
        public string? TypeRetrait { get; set; }

        // Pour Transfert
        public string? CompteDestinataire { get; set; }
    }
}
