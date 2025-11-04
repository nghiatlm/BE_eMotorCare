


using System.Text;

namespace eMotoCare.BO.Common.src
{
    public class Utils
    {
        private readonly Random _random = new Random();
        public delegate Task<bool> CheckCodeExistsDelegate(string code);

        public Task<string> GenerateUniqueBrokerCodeAsync(string prefix)
        {
            const int codeLength = 8;

            string randomDigits = GenerateRandomDigits(codeLength);
            string code = prefix + randomDigits;

            return Task.FromResult(code);
        }
        private string GenerateRandomDigits(int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(_random.Next(0, 10));
            }
            return sb.ToString();
        }

        public Task<string> GeneratePartCodeAsync()
        {
            const int digitCount = 5;
            string digits = GenerateRandomDigits(digitCount);

            string code = $"PAR-{digits}";
            return Task.FromResult(code);
        }

        public Task<string> GenerateCodeAsync(string prefix, int digitCount = 5)
        {
            string digits = GenerateRandomDigits(digitCount);
            string code = $"{prefix}-{digits}";
            return Task.FromResult(code);
        }
    }
}
