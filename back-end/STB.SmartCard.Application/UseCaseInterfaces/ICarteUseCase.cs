using STB.SmartCard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface ICarteUseCase
    {
        Task<List<CardDto>> GetCartesByClientIdAsync(Guid clientId);
        Task ModifierEtatCarteAsync(CardUpdateEtatDto dto);
        Task UpdatePlafondsAsync(CardPlafondUpdateDto dto);
        Task<CarteStatsDto> GetCarteStatsByClientIdAsync(Guid clientId);



    }
}
