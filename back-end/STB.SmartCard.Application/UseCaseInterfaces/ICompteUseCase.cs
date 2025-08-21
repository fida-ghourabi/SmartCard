using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface ICompteUseCase
    {
        Task<decimal> GetSoldeTotalByClientIdAsync(Guid clientId);
    }
}