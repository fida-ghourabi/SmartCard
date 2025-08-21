using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DTOs
{
    public class CarteStatsDto
    {
        public Guid ClientId { get; set; }
        public int TotalCartes { get; set; }
        public int CartesActives { get; set; }
    }
}
