using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class CardPlafondUpdateDto
    {
        public Guid CarteId { get; set; }
        public decimal NouveauPlafondRetrait { get; set; }
        public decimal NouveauPlafondPaiement { get; set; }
    }
}
