using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace PotteryWorkshop.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    private readonly ILogger<QrCodeService> _logger;

    public QrCodeService(ILogger<QrCodeService> logger)
    {
        _logger = logger;
    }

    public async Task<string> GenerateQrCodeAsync(string data)
    {
        try
        {
            _logger.LogInformation("Generating QR code for data: {Data}", data);
            
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            
            var qrCodeBytes = qrCode.GetGraphic(20);
            var base64String = Convert.ToBase64String(qrCodeBytes);
            
            return await Task.FromResult($"data:image/png;base64,{base64String}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating QR code");
            throw;
        }
    }

    public string DecodeQrCode(string qrCodeData)
    {
        // QR code decoding would typically be done on the client side or using a dedicated library
        // This is a placeholder implementation
        return qrCodeData;
    }
}
