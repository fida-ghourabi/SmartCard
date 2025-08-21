using Microsoft.Extensions.DependencyInjection;
using STB.SmartCard.Application.Services.Email;
using STB.SmartCard.Application.Services.Sms;
using STB.SmartCard.Domain.RepositoryInterfaces;
using STB.SmartCard.Infrastructure.RepositoryImplementation;
using STB.SmartCard.Infrastructure.Services.Email;
using STB.SmartCard.Infrastructure.Services.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService (this IServiceCollection services)
        {
            services.AddScoped<ICarteRepository, CarteRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionPendingRepository, TransactionPendingRepository>();
            services.AddScoped<ICompteRepository, CompteRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddHttpClient<ISmsService, SmsService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
