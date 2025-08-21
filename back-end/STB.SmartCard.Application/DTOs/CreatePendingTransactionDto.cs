using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class CreatePendingTransactionDto
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public decimal Montant { get; set; }
        public string? Lieu { get; set; }
        public string? NomBanque { get; set; }
        public string? TypeRetrait { get; set; }
        public string? CompteDestinataire { get; set; }
        public string? SourcePaiement { get; set; }
        [Required]
        public Guid CarteId { get; set; }
    }
}
