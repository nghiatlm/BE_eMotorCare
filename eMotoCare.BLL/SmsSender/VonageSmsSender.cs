using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace eMotoCare.BLL.SmsSender
{
    public class VonageSmsSender : ISmsSender
    {
        private readonly VonageClient _client;
        private readonly string _fromNumber;


        public VonageSmsSender(string apiKey, string apiSecret, string fromNumber = "Vonage")
        {
            var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
            _client = new VonageClient(credentials);
            _fromNumber = fromNumber;
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            var response = await _client.SmsClient.SendAnSmsAsync(new SendSmsRequest
            {
                To = phoneNumber,
                From = _fromNumber,
                Text = $"{otp}"
            });

            if (response.Messages[0].Status != "0")
            {
                throw new Exception($"Failed to send SMS: {response.Messages[0].ErrorText}");
            }
        }
    }
}
