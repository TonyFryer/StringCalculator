namespace CalculatorService.Exceptions
{
    /// <summary>
    /// Custom exception for the case of negative values being present. 
    /// </summary>
    public class NegativeValueException : Exception
    {
        public NegativeValueException() : base() { }

        public NegativeValueException(string? message) : base(message) { }

        public NegativeValueException(string? message, Exception? innerException) : base(message, innerException) {}
    }
}
