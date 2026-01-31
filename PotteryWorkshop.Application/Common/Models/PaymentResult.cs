namespace PotteryWorkshop.Application.Common.Models;

public class PaymentResult
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public string? GatewayResponse { get; set; }
    public string? PaymentUrl { get; set; }
}
