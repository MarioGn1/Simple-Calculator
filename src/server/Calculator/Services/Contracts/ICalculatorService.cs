using Calculator.Services.Models;

namespace Calculator.Services.Contracts;

public interface ICalculatorService
{
    Task<ServiceResult<double>> Calculate(string expression);
}
