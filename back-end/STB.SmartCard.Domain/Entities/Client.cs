using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public required string Nom { get; set; } 
        public required string Prenom { get; set; }
        

        public required string UserId { get; set; }
        public ApplicationUser User { get; set; }


        // Navigation properties
        public ICollection<Compte> Comptes { get; set; } = new List<Compte>();
        //public ICollection<Carte> Cartes { get; set; }
    }
}
