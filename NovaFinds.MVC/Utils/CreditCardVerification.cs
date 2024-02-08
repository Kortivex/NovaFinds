// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreditCardVerification.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CreditCardVerification type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Utils
{
    using System.Globalization;

    /// <summary>
    /// The credit card verification.
    /// </summary>
    public static class CreditCardVerification
    {
        /// <summary>
        /// The is valid card number for string input.
        /// </summary>
        /// <param name="cardNumber">
        /// The card number.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsValidCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", string.Empty);

            var sumTotal = 0;
            var isSecondDigit = false;

            // Iterate through the digits of the card from right to left.
            for (var i = cardNumber.Length - 1; i >= 0; i--){
                var digit = cardNumber[i] - '0';

                if (isSecondDigit){
                    digit *= 2;

                    // If the double of the digit is greater than 9, add 9 to it.
                    if (digit > 9){ digit -= 9; }
                }

                sumTotal += digit;
                isSecondDigit = !isSecondDigit;
            }

            // If the total sum is a multiple of 10, the number is valid.
            return (sumTotal % 10) == 0;
        }
    }
}