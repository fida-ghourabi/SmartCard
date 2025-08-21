using Microsoft.AspNetCore.Identity;
using STB.SmartCard.Application.DTOs.Auth;
using STB.SmartCard.Application.Services.Auth;
using STB.SmartCard.Application.UseCaseInterfaces;
using STB.SmartCard.Domain.Entities;
using STB.SmartCard.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.UseCaseImplementation
{
    public class AuthUseCase : IAuthUseCase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IClientRepository _clientRepo;

        public AuthUseCase(UserManager<ApplicationUser> userManager, IJwtTokenService jwtService, IClientRepository clientRepo)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _clientRepo = clientRepo;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            var client = new Client
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Nom = dto.Nom,
                Prenom = dto.Prenom
            };

            await _clientRepo.AddAsync(client);

            return "Inscription réussie.";
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Email ou mot de passe incorrect.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);
            var client = await _clientRepo.GetByUserIdAsync(user.Id);

            return new LoginResponseDto
            {
                Token = token,
                Nom = client.Nom,
                Prenom = client.Prenom,
                Phone = user.PhoneNumber
            };
        }
    }
}
