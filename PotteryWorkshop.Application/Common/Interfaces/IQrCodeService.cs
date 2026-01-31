namespace PotteryWorkshop.Application.Common.Interfaces;

public interface IQrCodeService
{
    Task<string> GenerateQrCodeAsync(string data);
    string DecodeQrCode(string qrCodeData);
}
