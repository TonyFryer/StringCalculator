using CalculatorService.Exceptions;
using FluentAssertions;

namespace CalculatorService.Test
{
    [TestClass]
    public class CalculatorService_Add_Should
    {
        private static CalculatorService? _calculatorService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _calculatorService = new CalculatorService();
        }

        [TestMethod]
        public void ReturnSumOfOneAddend()
        {
            _calculatorService?.Add("20").Should()
                .NotBeNullOrEmpty("The equation/sum was null or empty.")
                .And.BeEquivalentTo("20 = 20", "The equation/sum was not what was expected.")
                ;
        }

        [TestMethod]
        [DataRow("20,1", "20+1 = 21", DisplayName = "Should add two integers.")]
        [DataRow("20,1,11,6", "20+1+11+6 = 38", DisplayName = "Should add more than two integers.")]
        [DataRow("0,0", "0+0 = 0", DisplayName = "Should handle two zeroes.")]
        [DataRow("0,", "0+0 = 0", DisplayName = "Should handle missing integers.")]
        [DataRow("5,2.66", "5+0 = 5", DisplayName = "Should ignore decimals.")]
        [DataRow("1,8739875934784", "1+0 = 1", DisplayName = "Should handle larger than int32.")]
        [DataRow("1,1000,1001", "1+1000+0 = 1001", DisplayName = "Should treat values greater than 1000 as zero.")]
        [DataRow("1\\n1,1", "1+1+1 = 3", DisplayName = "Should handle newline character as alternate separator.'")]
        public void ReturnSumOfTwoOrMoreAddends(string addends, string expectedSum)
        {
            _calculatorService?.Add(addends).Should()
                .NotBeNullOrEmpty("The equation/sum was null or empty.")
                .And.BeEquivalentTo(expectedSum, $"The equation/sum for `{addends}` was not what was expected.")
                ;
        }

        [TestMethod]
        [Ignore] // This test is obsolete given business requirement #2.
        public void ThrowExceptionWhenMoreThanTwoAddends()
        {
            Action act = () => _calculatorService?.Add("20,1,3");
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ThrowExceptionWhenNegativeNumbersPresent()
        {
            Action act = () => _calculatorService?.Add("20,-1,-3");
            act.Should().Throw<NegativeValueException>()
                .Where(e => e.Message.Contains("-1,-3"));
        }

        [TestMethod]
        public void ConvertMissingAddendsToZero()
        {
            _calculatorService?.Add(string.Empty).Should()
                .NotBeNullOrEmpty("The equation/sum was null or empty.")
                .And.BeEquivalentTo("0 = 0", "The equation/sum was not what was expected.")
                ;
        }

        [TestMethod]
        public void ConvertInvalidAddendsToZero()
        {
            _calculatorService?.Add("20,ABC123").Should()
                .NotBeNullOrEmpty("The equation/sum was null or empty.")
                .And.BeEquivalentTo("20+0 = 20", "The equation/sum was not what was expected.")
                ;
        }

        [TestMethod]
        [DataRow("//!\\n20,1!3", "20+1+3 = 24", DisplayName = "Should support '!' as a delimiter.")]
        [DataRow("//6\\n20,163", "20+1+3 = 24", DisplayName = "Should a number as a delimiter.")]
        [DataRow("//*\\n20,1*3\\n6", "20+1+3+6 = 30", DisplayName = "Should not confuse number separator from newline delimiters.")]
        public void SupportCustomDelimiters(string addends, string expectedSum)
        {
            _calculatorService?.Add(addends).Should()
                .NotBeNullOrEmpty("The equation/sum was null or empty.")
                .And.BeEquivalentTo(expectedSum, $"The equation/sum for `{addends}` was not what was expected.")
                ;
        }

        [TestMethod]
        public void ThrowExceptionWhenPoorlyFormedCustomDelimiter()
        {
            Action act = () => _calculatorService?.Add("//\\n4,5,6");
            act.Should().Throw<InvalidInputException>()
                .Where(e => e.Message.Contains("Custom delimiter declaration was invalid."));
        }
    }
}