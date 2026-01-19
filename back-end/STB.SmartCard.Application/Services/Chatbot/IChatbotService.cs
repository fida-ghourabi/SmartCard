using STB.SmartCard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STB.SmartCard.Application.Services.Chatbot
{
    public interface IChatbotService
    {
        Task<ChatResponseDto> AskAsync(Guid clientId, string userMessage);
    }
}
