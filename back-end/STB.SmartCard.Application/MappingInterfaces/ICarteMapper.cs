using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.MappingInterfaces
{
    public interface ICarteMapper
    {
        CardDto ToCardDto(Carte carte);

    }
}
