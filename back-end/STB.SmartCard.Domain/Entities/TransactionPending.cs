using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.Entities
{
    public class TransactionPending
    {
        public Guid Id { get; set; }
        public required TransactionType Type { get; set; }
        public required DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Montant { get; set; }

        public string? Lieu { get; set; }
        public string? NomBanque { get; set; }
        public RetraitType? TypeRetrait { get; set; }
        public string? CompteDestinataire { get; set; }
        public string? SourcePaiement { get; set; }

        public Guid CarteId { get; set; }
        public Carte Carte { get; set; }

        // OTP
        public required string OtpCode { get; set; }
        public required DateTime Expiration { get; set; }
        public bool IsValidated { get; set; } = false;
    }
}
