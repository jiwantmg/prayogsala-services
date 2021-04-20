namespace PragyoSala.Services.Dtos
{
    public class VerifyPayment
    {
        public string OrderNo { get; set; }
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string Token { get; set; }
        public string Idx { get; set; }
        public string Mobile { get; set; }
        public int ProductIdentity { get; set; }
        public string ProductUrl { get; set; }
        public string WidgetId { get; set; }
        public string PaymentMethod { get; set; }
    }
}