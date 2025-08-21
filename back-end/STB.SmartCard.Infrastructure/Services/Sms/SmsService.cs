using STB.SmartCard.Application.Services.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace STB.SmartCard.Infrastructure.Services.Sms
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;

        public SmsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendOtpSmsAsync(string mobile, string otp)
        {
            var payload = new
            {
                message = $"Votre code OTP est : {otp}",
                mobile = mobile
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "55c87b41825244d7b0299f66e3bda7f6");

            var response = await _httpClient.PostAsync("https://openbank.stb.com.tn/api/students/subscription/sendsms", content);
            response.EnsureSuccessStatusCode(); // Lève exception si erreur HTTP
        }
    }
}
