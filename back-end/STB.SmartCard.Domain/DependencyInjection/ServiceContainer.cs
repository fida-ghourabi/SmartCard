using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Domain.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddDomainService(this IServiceCollection services)
        {
            return services;
        }
    }
}
