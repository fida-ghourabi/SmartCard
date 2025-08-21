using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.MappingInterfaces;
using STB.SmartCard.Application.Services.Email;
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
    public class CarteUseCase : ICarteUseCase
    {
        private readonly ICarteRepository _carteRepository;
        private readonly ICarteMapper _carteMapper;
        private readonly IEmailService _emailService;


        public CarteUseCase(ICarteRepository carteRepository, ICarteMapper carteMapper, IEmailService emailService)
        {
            _carteRepository = carteRepository;
            _carteMapper = carteMapper;
            _emailService = emailService;
        }

        public async Task<List<CardDto>> GetCartesByClientIdAsync(Guid clientId)
        {
            var cartes = await _carteRepository.GetCartesByClientIdAsync(clientId);
            return cartes.Select(_carteMapper.ToCardDto).ToList();
        }

        public async Task ModifierEtatCarteAsync(CardUpdateEtatDto dto)
        {
            var carte = await _carteRepository.GetByIdWithCompteAndClientAsync(dto.CarteId);
            if (carte == null)
                throw new Exception("Carte non trouvée");

            if (dto.NouvelEtat != "Active" && dto.NouvelEtat != "Desactive" && dto.NouvelEtat != "Bloque")
                throw new Exception("État invalide. Les états valides sont: Active, Desactive, Bloque.");

            if (!Enum.TryParse<EtatCarteEnum>(dto.NouvelEtat, true, out var nouvelEtat))
            {
                throw new Exception("État de carte invalide");
            }
            carte.EtatCarte = nouvelEtat;

            await _carteRepository.UpdateAsync(carte);
            // Email de notification
            var email = carte.Compte.Client.User.Email;
            await _emailService.SendEmailAsync(email,
                "Mise à jour de votre carte",
                $"Votre carte {carte.NumeroCarte} est maintenant en état : {nouvelEtat}");
        
        }
        public async Task UpdatePlafondsAsync(CardPlafondUpdateDto dto)
        {
            var carte = await _carteRepository.GetByIdAsync(dto.CarteId);
            if (carte == null)
                throw new Exception("Carte introuvable");

            if (dto.NouveauPlafondRetrait > carte.PlafondRetraitMax)
                throw new Exception("Le plafond de retrait dépasse le maximum autorisé");

            if (dto.NouveauPlafondPaiement > carte.PlafondPaiementMax)
                throw new Exception("Le plafond de paiement dépasse le maximum autorisé");

            carte.PlafondRetrait = dto.NouveauPlafondRetrait;
            carte.PlafondPaiement = dto.NouveauPlafondPaiement;

            await _carteRepository.UpdatePlafondAsync(carte);
        }

        public async Task<CarteStatsDto> GetCarteStatsByClientIdAsync(Guid clientId)
        {
            var cartes = await _carteRepository.GetCartesByClientIdAsync(clientId);

            return new CarteStatsDto
            {
                ClientId = clientId,
                TotalCartes = cartes.Count,
                CartesActives = cartes.Count(c => c.EtatCarte.ToString() == "Active")
            };
        }

    }
}
