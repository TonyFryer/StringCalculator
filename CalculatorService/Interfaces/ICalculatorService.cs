namespace CalculatorService.Interfaces
{
    public interface ICalculatorService
    {
        string FunctionalityDescription { get; }

        string Add(string addends);
    }
}
