namespace AgendaUni.Common;

public class ServiceResult
{
    public bool IsSuccess { get; }
    public string Message { get; }

    protected ServiceResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static ServiceResult Success(string message = "Sucesso.") => new(true, message);
    public static ServiceResult Failure(string message = "Erro.") => new(false, message);
}


public class ServiceResult<T> : ServiceResult
{
    public T Data { get; }

    private ServiceResult(bool isSuccess, string message, T data): base(isSuccess, message)
    {
        Data = data;
    }

    public static ServiceResult<T> Success(T data, string message = "Sucesso.") =>
        new(true, message, data);

    public static ServiceResult<T> Failure(string message) =>
        new(false, message, default);
}
