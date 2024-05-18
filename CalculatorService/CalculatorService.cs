using CalculatorService.Interfaces;

namespace CalculatorService
{
    /// <summary>
    /// A simple calculator class for the Restaurant365 coding challenge.
    /// </summary>
    public class CalculatorService : ICalculatorService
    {
        /// <summary>
        /// Provides a string description of the current functionality for the host console application.
        /// </summary>
        public string FunctionalityDescription
        {
            get { return "To use the Add function, please type two, whole, comma-separated numbers (example: '5,5') and press enter. Other values will be treated as zero value."; }
        }

        /// <summary>
        /// Performs the addition function of the calculator.
        /// </summary>
        /// <param name="addends">A comma-separated string of integers to add together.</param>
        /// <returns>A string value representing the sum of the addends.</returns>
        public string Add(string addends)
        {
            var values = ValidateInputs(addends);
            var sum = 0;
            foreach (var value in values) 
                if (int.TryParse(value, out int intValue))
                    sum += intValue;

            return sum.ToString();
        }

        /// <summary>
        /// Performs business rule validation on the input string.
        /// </summary>
        /// <param name="addends">A comma-separated string of integers to add together.</param>
        /// <returns>A string array of the separated values.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a business rule has been violated.</exception>
        private string[] ValidateInputs(string addends)
        {
            var values = addends.Split(',');
            if (values.Length > 2) // Could use FluentValidator, but that feels like overkill for this scenario.
                throw new InvalidOperationException("The input cannot exceed two values to add.");

            return values;
        }
    }
}
