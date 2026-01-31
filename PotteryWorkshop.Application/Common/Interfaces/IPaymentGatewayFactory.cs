using PotteryWorkshop.Domain.Enums;

namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IPaymentGatewayFactory
{
    IPaymentGateway GetPaymentGateway(PaymentGateway gateway);
}
