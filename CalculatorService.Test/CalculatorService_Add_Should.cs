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
        [DataRow("5,-1", "4", DisplayName = "Should handle negative numbers.")]
        [DataRow("0,0", "0", DisplayName = "Should handle two zeroes.")]
        [DataRow("0,-1", "-1", DisplayName = "Should handle zeroes and negatives.")]
        [DataRow("0,", "0", DisplayName = "Should handle missing integers.")]
        [DataRow("5,2.66", "5", DisplayName = "Should ignore decimals.")]
        [DataRow("1,8739875934784", "8739875934785", DisplayName = "Should handle larger than int32.")]
        public void ReturnSumOfTwoAddends(string addends, string expectedSum)
        {
            _calculatorService?.Add(addends).Should()
                .NotBeNullOrEmpty("The sum was null or empty.")
                .And.BeEquivalentTo(expectedSum, $"The sum for `{addends}` was not what was expected.")
                ;
        }

        [TestMethod]
        public void ThrowExceptionWhenMoreThanTwoAddends()
        {
            Action act = () => _calculatorService?.Add("20,1,3");
            act.Should().Throw<InvalidOperationException>();
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