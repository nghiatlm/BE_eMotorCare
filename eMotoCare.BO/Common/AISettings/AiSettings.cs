using System;

namespace eMotoCare.BO.Common.AISettings
{
    public class AiSettings
    {
        public string Provider { get; set; } = "Gemini";
        public string? OpenAIApiKey { get; set; }
        public string? GeminiApiKey { get; set; }
    }
}
