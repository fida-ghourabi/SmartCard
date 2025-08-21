using STB.SmartCard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseInterfaces
{
    public interface IClientUseCase
    {
        Task<Client?> GetClientByUserIdAsync(string userId);
    }
}
