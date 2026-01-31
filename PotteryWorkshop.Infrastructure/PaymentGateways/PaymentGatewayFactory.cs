using PotteryWorkshop.Application.Common.Interfaces;
using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Infrastructure.PaymentGateways;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentGateway GetPaymentGateway(PaymentGateway gateway)
    {
        return gateway switch
        {
            PaymentGateway.Cashfree => (IPaymentGateway)_serviceProvider.GetService(typeof(CashfreePaymentGateway))!,
            PaymentGateway.Razorpay => (IPaymentGateway)_serviceProvider.GetService(typeof(RazorpayPaymentGateway))!,
            _ => throw new ArgumentException($"Payment gateway {gateway} is not supported")
        };
    }
}
