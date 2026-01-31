using Microsoft.Extensions.Logging;
using PotteryWorkshop.Application.Common.Interfaces;

namespace PotteryWorkshop.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly string _uploadPath;

    public FileStorageService(ILogger<FileStorageService> logger)
    {
        _logger = logger;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        try
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);
            
            using var fileStreamOutput = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOutput);
            
            _logger.LogInformation("File uploaded successfully: {FileName}", uniqueFileName);
            
            return $"/uploads/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", fileName);
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        try
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_uploadPath, fileName);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("File deleted successfully: {FileName}", fileName);
            }
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileUrl}", fileUrl);
            throw;
        }
    }
}
