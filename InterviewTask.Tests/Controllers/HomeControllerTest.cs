using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterviewTask.Tests.Controllers
{
    using ServiceReference1;

    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// The client object
        /// </summary>
        private static FormatClient _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _client = new FormatClient();
        }

        [TestMethod]
        public void FormatPrice_WithInvalidNumberFormat_ShouldReturnInValidErrorMessage()
        {
            //Arrange
            var numberAsString = "23.45.67";
            var expectedResult = "INVALID NUMBER OR ZERO";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithString_ShouldReturnInValidErrorMessage()
        {
            //Arrange
            var numberAsString = "abc";
            var expectedResult = "INVALID NUMBER OR ZERO";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithStringNumberCombination_ShouldReturnInValidErrorMessage()
        {
            //Arrange
            var numberAsString = "abc123";
            var expectedResult = "INVALID NUMBER OR ZERO";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithInValidNumber_ShouldReturnInValidErrorMessage()
        {
            //Arrange
            var numberAsString = "-23-23";
            var expectedResult = "INVALID NUMBER OR ZERO";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithValidDigits_ShouldReturnNumberInWords()
        {
            //Arrange
            var numberAsString = "123";
            var expectedResult = "ONE HUNDRED AND TWENTY THREE  DOLLARS ONLY";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithSingleDigit_ShouldReturnNumberInWords()
        {
            //Arrange
            var numberAsString = "1";
            var expectedResult = "ONE  DOLLARS ONLY";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithOnlyZeroDigit_ShouldReturnNumberInWords()
        {
            //Arrange
            var numberAsString = "0";
            var expectedResult = "ZERO";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void FormatPrice_WithDecimal_ShouldReturnNumberInWords()
        {
            //Arrange
            var numberAsString = "123.45";
            var expectedResult = "ONE HUNDRED AND TWENTY THREE DOLLARS AND FOURTY FIVE CENTS ONLY";

            //Act
            var actualResult = _client.FormatPrice(numberAsString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
