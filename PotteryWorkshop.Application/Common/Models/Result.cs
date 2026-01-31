namespace PotteryWorkshop.Application.Common.Models;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static Result<T> Succeed(T data) => new() { Success = true, Data = data };
    public static Result<T> Fail(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
    public static Result<T> Fail(List<string> errors) => new() { Success = false, Errors = errors };
}
