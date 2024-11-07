namespace Emne_7___Arbeidskrav_2.Features.Users;

public class ServiceResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; }

    protected ServiceResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static ServiceResult Success(string message = "") => new ServiceResult(true, message);
    public static ServiceResult Failure(string message) => new ServiceResult(false, message);
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; private set; }

    private ServiceResult(bool success, string message, T? data) : base(success, message)
    {
        Data = data;
    }

    public static ServiceResult<T> Success(T data) => new ServiceResult<T>(true, "", data);
    public static ServiceResult<T> Failure(string message) => new ServiceResult<T>(false, message, default);
}