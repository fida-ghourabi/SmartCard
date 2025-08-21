using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class CardUpdateEtatDto
    {
        public Guid CarteId { get; set; }
        public string NouvelEtat { get; set; } = null!;
    }
}
