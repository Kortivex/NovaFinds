namespace NovaFinds.MVC.Test
{
    using Utils;

    [TestFixture]
    public class CreditCardVerificationTests
    {
        [TestCase("4111111111111111", true)]
        [TestCase("5555555555554444", true)]
        [TestCase("1234567812345670", true)]
        [TestCase("378282246310005", true)]
        [TestCase("6011111111111117", true)]
        [TestCase("5105105105105100", true)]
        [TestCase("4111111111111", false)]
        [TestCase("4012888888881881", true)]
        [TestCase("4222222222222", true)]
        [TestCase("76009244561", false)]
        public void IsValidCardNumber_VariousCards_ReturnsExpectedResult(string cardNumber, bool expected)
        {
            var result = CreditCardVerification.IsValidCardNumber(cardNumber);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void IsValidCardNumber_ContainsLetters_ReturnsFalse()
        {
            var result = CreditCardVerification.IsValidCardNumber("4111a11111111111");
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidCardNumber_ContainsSpecialCharacters_ReturnsFalse()
        {
            var result = CreditCardVerification.IsValidCardNumber("4111-1111-1111-1111");
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidCardNumber_SpacesBetweenNumbers_HandledCorrectly()
        {
            var result = CreditCardVerification.IsValidCardNumber("4111 1111 1111 1111");
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidCardNumber_LeadingAndTrailingSpaces_TrimmedCorrectly()
        {
            var result = CreditCardVerification.IsValidCardNumber(" 4111111111111111 ");
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidCardNumber_InvalidLength_ReturnsFalse()
        {
            var result = CreditCardVerification.IsValidCardNumber("41111111111111"); // 14 digits
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidCardNumber_ExactLengthBoundary_ReturnsTrue()
        {
            // Assuming the test card number of valid length passes Luhn check
            var valid16DigitCardNumber = "4111111111111111"; // 16 digits
            var result = CreditCardVerification.IsValidCardNumber(valid16DigitCardNumber);
            Assert.That(result, Is.True);
        }
    }
}