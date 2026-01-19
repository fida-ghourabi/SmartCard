using System.Collections.Generic;

namespace STB.SmartCard.Application.DTOs
{
    public class ChatRequestDto
    {
        public string Message { get; set; } = null!;
    }

    public class ChatResponseDto
    {
        public string Reply { get; set; } = null!;
        public List<string> Suggestions { get; set; } = new();
    }
}
