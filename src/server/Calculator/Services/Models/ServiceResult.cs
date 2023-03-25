namespace Calculator.Services.Models;

public sealed class ServiceResult<T> where T : struct
{
    private readonly List<string> _errorMessages;

    public ServiceResult()
    {
        _errorMessages = new List<string>();
    }

    public T Data { get; private set; }

    public bool Success => _errorMessages.Count == 0;

    public Task SetResult(T result)
    {
        Data = result;

        return Task.CompletedTask;
    }

    public Task AddMessage(string message)
    {
        _errorMessages.Add(message);

        return Task.CompletedTask;
    }

    public IReadOnlyCollection<string> ErrorMessages => _errorMessages;
}
