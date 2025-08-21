using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.Entities
{
    public class Compte
    {
        public Guid Id { get; set; }
        public required string NumCompte { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal Solde { get; set; }

        // FK
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        // Navigation
        public ICollection<Carte> Cartes { get; set; } = new List<Carte>();
    }
}
