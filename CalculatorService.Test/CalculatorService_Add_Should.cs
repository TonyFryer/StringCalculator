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
        [DataRow("20,1", "21")]
        [DataRow("22,22", "44")]
        [DataRow("5,-1", "4")]
        [DataRow("0,0", "0")]
        [DataRow("0,-1", "-1")]
        [DataRow("0,", "0")]
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