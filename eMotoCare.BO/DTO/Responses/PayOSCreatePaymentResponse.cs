namespace eMotoCare.BO.DTO.Responses
{
    public class PayOSCreatePaymentResponse
    {
        public string CheckoutUrl { get; set; } = string.Empty;
        public string TransactionCode { get; set; } = string.Empty;
    }
}
