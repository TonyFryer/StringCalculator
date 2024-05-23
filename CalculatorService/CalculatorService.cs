using CalculatorService.Exceptions;
using CalculatorService.Interfaces;
using System.Text.RegularExpressions;

namespace CalculatorService
{
    /// <summary>
    /// A simple calculator class for the Restaurant365 coding challenge.
    /// </summary>
    public class CalculatorService : ICalculatorService
    {
        /// <summary>
        /// Provides a string description of the current functionality of the CalculatorService for the host console application.
        /// </summary>
        public string FunctionalityDescription
        {
            get { return "To use the Add function, please type two, whole, comma-separated numbers (example: '5,5') and press enter. Other values will be treated as zero value. " +
                    @"Negative values are not allowed. Note: The newline character (\n) is allowed as an alternate separator. Custom, single-character separators can be declared " +
                    @"using this syntax: //{delimiter}\n{numbers}. Additionally, multiple custom delimiters of various length can be provided using this format: //[{delimiter}]...\n{numbers}." +
                    @"Example: //[*][!!][r9r]\n11r9r22*hh*33!!44"; }
        }

        /// <summary>
        /// Performs the addition function of the calculator.
        /// </summary>
        /// <param name="addends">A comma-separated string of integers to add together.</param>
        /// <returns>A string value representing the sum of the addends.</returns>
        public string Add(string addends)
        {
            var values = ProcessInput(addends);
            var sum = 0m;
            foreach (var value in values) 
                sum += value;

            var equation = $"{string.Join("+", values)} = {sum}";
            return equation;
        }

        /// <summary>
        /// Orchestrates data clean up, conversion, and business rule validation on the input string.
        /// </summary>
        /// <param name="addends">A delimiter-separated string of values to add together.</param>
        /// <returns>A string array of the processed and separated values.</returns>
        /// <exception cref="NegativeValueException">Thrown when a negative numeric value has been provided.</exception>
        /// <exception cref="InvalidInputException">Thrown when the user's input is formatted in error.</exception>
        private long[] ProcessInput(string addends)
        {
            var separators = GetDelimiters(addends);
            var rawAddends = GetAddends(addends);
            var values = rawAddends.Split(separators, StringSplitOptions.None);

            var addendValues = new List<long>();
            foreach (var value in values)
            {
                var addend = ProcessAddend(value);
                addendValues.Add(addend);
            }

            // Test for negative values.
            if (addendValues.Any(v => v < 0))
                throw new NegativeValueException($"Values cannot include negative numbers. Invalid values: {string.Join(",", addendValues.Where(v => v < 0).ToArray())}");

            return addendValues.ToArray();
        }

        /// <summary>
        /// Processes individual inputs by the user. Values over 1000 should be considered a zero. Non-numeric values will automatically be a zero.
        /// </summary>
        /// <param name="value">The user input value to be processed.</param>
        /// <returns>A numeric value.</returns>
        private long ProcessAddend(string? value)
        {
            _ = long.TryParse(value, out long intValue);
            if (intValue > 1000)
                intValue = 0;

            return intValue;
        }

        /// <summary>
        /// A single-character, custom delimiter can be provided using this format: //{delimiter}\n{numbers}. Additionally, 
        /// multiple custom delimiters of various length can be provided using this format: //[{delimiter}]...\n{numbers}.
        /// Example: //[*][!!][r9r]\n11r9r22*hh*33!!44
        /// </summary>
        /// <param name="addends">The user input value to be processed.</param>
        /// <returns>A string array of delimiters.</returns>
        /// <exception cref="InvalidInputException">Thrown when the user's input is formatted in error.</exception>
        private string[] GetDelimiters(string addends)
        {
            var separators = new List<string> { ",", "\\n" };
            if (addends.StartsWith("//"))
            {
                try
                {
                    // Let's first determine if we have one or more custom delimiters, which are of any length.
                    Regex regex = new Regex(@"\[(.*?)\]");
                    MatchCollection matches = regex.Matches(addends);
                    if (matches.Count > 0) 
                    {
                        MatchCollection delimiterMatches = regex.Matches(addends.Split("\\n").First());
                        foreach (Match match in matches)
                            separators.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        // We're taking a single character as the custom delimiter. 
                        var customDelimiter = addends.Split("\\n").First().Substring(2, 1).Single().ToString();
                        separators.Add(customDelimiter);
                    }
                }
                catch (ArgumentOutOfRangeException aor)
                {
                    // This would occur only if the user failedtp provide a character after the '//'.
                    throw new InvalidInputException("Custom delimiter declaration was invalid.", aor);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return separators.ToArray();
        }

        /// <summary>
        /// If a custom delimiter has been declared, split the declaration off of the addends and just return the "numbers".
        /// </summary>
        /// <param name="addends">The input string from the user.</param>
        /// <returns>The "numbers" portion of the user input.</returns>
        private string GetAddends(string addends)
        {
            if (addends.StartsWith("//"))
                addends = string.Join(",", addends.Split("\\n").Skip(1));

            return addends;
        }
    }
}
