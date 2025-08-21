using Microsoft.Extensions.DependencyInjection;
using STB.SmartCard.Application.MappingImplementation;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Application.Services.Auth;
using STB.SmartCard.Application.UseCaseImplementation;
using STB.SmartCard.Application.UseCaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // UseCases
            services.AddScoped<ICarteUseCase, CarteUseCase>();
            services.AddScoped<ICompteUseCase, CompteUseCase>();
            services.AddScoped<ITransactionUseCase, TransactionUseCase>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthUseCase, AuthUseCase>();
            services.AddScoped<IClientUseCase, ClientUseCase>();
            services.AddScoped<ITransactionOtpUseCase, TransactionOtpUseCase>();

            // Mappers
            services.AddScoped<ICarteMapper, CarteMapper>();
            services.AddScoped<ITransactionMapper, TransactionMapper>();
            services.AddScoped<ITransactionCreateMapper, TransactionCreateMapper>();
            services.AddScoped<ITransactionPendingMapper, TransactionPendingMapper>();


            return services;
        }
    }
}
