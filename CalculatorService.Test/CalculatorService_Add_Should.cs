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
                .NotBeNullOrEmpty("The sum was null or empty.")
                .And.BeEquivalentTo("20", "The sum was not what was expected.")
                ;
        }

        [TestMethod]
        [DataRow("20,1", "21", DisplayName = "Should add two integers.")]
        [DataRow("20,1,11,6", "38", DisplayName = "Should add more than two integers.")]
        [DataRow("0,0", "0", DisplayName = "Should handle two zeroes.")]
        [DataRow("0,", "0", DisplayName = "Should handle missing integers.")]
        [DataRow("5,2.66", "5", DisplayName = "Should ignore decimals.")]
        [DataRow("1,8739875934784", "1", DisplayName = "Should handle larger than int32.")]
        [DataRow("1,1000,1001", "1001", DisplayName = "Should treat values greater than 1000 as zero.")]
        [DataRow("1\\n1,1", "3", DisplayName = "Should handle newline character as alternate separator.'")]
        public void ReturnSumOfTwoAddends(string addends, string expectedSum)
        {
            _calculatorService?.Add(addends).Should()
                .NotBeNullOrEmpty("The sum was null or empty.")
                .And.BeEquivalentTo(expectedSum, $"The sum for `{addends}` was not what was expected.")
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
                .NotBeNullOrEmpty("The sum was null or empty.")
                .And.BeEquivalentTo("0", "The sum was not what was expected.")
                ;
        }

        [TestMethod]
        public void ConvertInvalidAddendsToZero()
        {
            _calculatorService?.Add("20,ABC123").Should()
                .NotBeNullOrEmpty("The sum was null or empty.")
                .And.BeEquivalentTo("20", "The sum was not what was expected.")
                ;
        }
    }
}