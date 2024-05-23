namespace CalculatorService.Exceptions
{
    /// <summary>
    /// Exception thrown by CalculatorService if errors are encountered due to user input.
    /// </summary>
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base() { }

        public InvalidInputException(string? message) : base(message) { }

        public InvalidInputException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
