using CalculatorService.Exceptions;
using CalculatorService.Interfaces;
using System.Linq;

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
            get { return "To use the Add function, please type two, whole, comma-separated numbers (example: '5,5') and press enter. Other values will be treated as zero value. " +
                    "Negative values are not allowed. Note: The newline character (\\n) is allowed as an alternate separator."; }
        }

        /// <summary>
        /// Performs the addition function of the calculator.
        /// </summary>
        /// <param name="addends">A comma-separated string of integers to add together.</param>
        /// <returns>A string value representing the sum of the addends.</returns>
        public string Add(string addends)
        {
            var values = ValidateInputs(addends);
            var sum = 0m;
            foreach (var value in values) 
                if (long.TryParse(value, out long intValue))
                    sum += intValue;

            var equation = $"{string.Join("+", values)} = {sum.ToString()}";
            return equation;
        }

        /// <summary>
        /// Performs business rule validation on the input string.
        /// </summary>
        /// <param name="addends">A comma-separated string of integers to add together.</param>
        /// <returns>A string array of the processed and separated values.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a business rule has been violated.</exception>
        private string[] ValidateInputs(string addends)
        {
            var separators = new string[] { ",", "\\n" };
            var values = addends.Split(separators, StringSplitOptions.None);

            var negativeValues = new List<string>();
            var addendValues = new List<string>();
            foreach (var value in values)
            {
                _ = long.TryParse(value, out long intValue);
                // Values over 1000 should be considered a zero.
                if (intValue > 1000)
                    intValue = 0;
                // Make a clean list of values to add. Invalid values are considered a zero.
                addendValues.Add(intValue.ToString());
                // Test for negative values.
                if (intValue < 0)
                    negativeValues.Add(intValue.ToString());
            }

            if (negativeValues.Count > 0) 
                throw new NegativeValueException($"Values cannot include negative numbers. Invalid values: {string.Join(",", negativeValues)}");

            return addendValues.ToArray();
        }
    }
}
