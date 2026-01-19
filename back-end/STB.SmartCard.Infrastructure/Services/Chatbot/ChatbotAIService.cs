using Microsoft.Extensions.Configuration;
using STB.SmartCard.Application.DTOs;
using STB.SmartCard.Application.Services.Chatbot;
using STB.SmartCard.Application.UseCaseInterfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace STB.SmartCard.Infrastructure.Services.Chatbot
{
    public class ChatbotAIService : IChatbotService
    {
        private readonly ICompteUseCase _compteUseCase;
        private readonly ICarteUseCase _carteUseCase;
        private readonly ITransactionUseCase _transactionUseCase;
        private readonly IHttpClientFactory _httpFactory;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ChatbotAIService(
            ICompteUseCase compteUseCase,
            ICarteUseCase carteUseCase,
            ITransactionUseCase transactionUseCase,
            IHttpClientFactory httpFactory)
        {
            _compteUseCase = compteUseCase;
            _carteUseCase = carteUseCase;
            _transactionUseCase = transactionUseCase;
            _httpFactory = httpFactory;
        }

        public async Task<ChatResponseDto> AskAsync(Guid clientId, string userMessage)
        {
            await _semaphore.WaitAsync();
            try
            {
                decimal soldeTotal = 0;
                try { soldeTotal = await _compteUseCase.GetSoldeTotalByClientIdAsync(clientId); } catch { }

                var cartes = await _carteUseCase.GetCartesByClientIdAsync(clientId);
                var lastTx = await _transactionUseCase.GetLastTransactionByClientIdAsync(clientId);

                var sb = new StringBuilder();
                sb.AppendLine("Tu es un assistant bancaire français, poli et concis.");
                sb.AppendLine($"Question du client : \"{userMessage}\"");
                sb.AppendLine();
                sb.AppendLine("Données client :");
                sb.AppendLine($"- Solde total : {soldeTotal} TND");
                sb.AppendLine($"- Nombre de cartes : {cartes?.Count ?? 0}");
                if (cartes != null && cartes.Any())
                {
                    var c = cartes.First();
                    sb.AppendLine($"- Exemple carte : numéro (masqué) {MaskCard(c.NumeroCarte)}, type {c.TypeCarte}, état {c.EtatCarte}, expir. {c.DateExpiration:dd/MM/yyyy}");
                }
                if (lastTx != null)
                    sb.AppendLine($"- Dernière transaction : {lastTx.Type} {lastTx.Montant} TND le {lastTx.Date:dd/MM/yyyy HH:mm}");

                sb.AppendLine();
                sb.AppendLine("Réponds UNIQUEMENT aux sujets comptes/cartes/transactions.");
                sb.AppendLine("Réponds en Français, clair et court (1-3 phrases).");
                sb.AppendLine();
                sb.AppendLine("Question :");
                sb.AppendLine(userMessage);

                // ✅ Utiliser le HttpClient nommé "openai"
                var client = _httpFactory.CreateClient("openai");

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "Vous êtes un assistant bancaire." },
                        new { role = "user", content = sb.ToString() }
                    },
                    max_tokens = 300,
                    temperature = 0.2
                };

                var json = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = await client.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);
                if (!resp.IsSuccessStatusCode)
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    return new ChatResponseDto { Reply = $"Erreur IA: {resp.StatusCode} - {err}" };
                }

                var respContent = await resp.Content.ReadAsStringAsync();
                using var sr = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(respContent));
                using var doc = await JsonDocument.ParseAsync(sr);
                var root = doc.RootElement;

                var reply = "Je n'ai pas pu générer de réponse.";
                if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                {
                    var msg = choices[0].GetProperty("message");
                    reply = msg.GetProperty("content").GetString() ?? reply;
                }

                return new ChatResponseDto
                {
                    Reply = reply.Trim(),
                    Suggestions = BuildSuggestions(userMessage)
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static string MaskCard(string numero)
        {
            if (string.IsNullOrEmpty(numero)) return "****";
            if (numero.Length <= 4) return new string('*', numero.Length);
            return new string('*', numero.Length - 4) + numero[^4..];
        }

        private static System.Collections.Generic.List<string> BuildSuggestions(string userMessage)
        {
            var lower = userMessage.ToLowerInvariant();
            var list = new System.Collections.Generic.List<string>();
            if (lower.Contains("solde"))
            {
                list.Add("Afficher mes comptes");
                list.Add("Dernières transactions");
                list.Add("Voir mes plafonds de carte");
            }
            else if (lower.Contains("carte"))
            {
                list.Add("Lister mes cartes actives");
                list.Add("Plafonds de paiement/retrait");
                list.Add("Opposer une carte");
            }
            else if (lower.Contains("transaction") || lower.Contains("dernier"))
            {
                list.Add("Exporter mes transactions");
                list.Add("Filtrer par date");
                list.Add("Voir les opérations en attente");
            }
            else
            {
                list.Add("Quel est mon solde ?");
                list.Add("Mes cartes");
                list.Add("Dernière transaction");
            }
            return list;
        }
    }
}
