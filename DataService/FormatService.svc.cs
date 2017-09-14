using System;

namespace DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FormatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FormatService.svc or FormatService.svc.cs at the Solution Explorer and start debugging.
    public class FormatService : IFormat
    {
        private const string _errorMessage = "INVALID NUMBER OR ZERO";

        /// <summary>
        /// Formats the price.
        /// </summary>
        /// <param name="priceAsString">The price.</param>
        /// <returns>Formatted price</returns>
        public string FormatPrice(string priceAsString)
        {
            if (!ValidateNumber(priceAsString))
            {
                return _errorMessage;
            }

            string priceInWords = null;
            if (priceAsString.Contains("-"))
            {
                priceInWords = "MINUS ";
                priceAsString = priceAsString.Substring(1, priceAsString.Length - 1);
            }

            if (priceAsString == "0")
            {
                return "ZERO";
            }

            return string.Concat(priceInWords, ConvertCurrencyToWords(priceAsString));
        }

        /// <summary>
        /// Validates the number.
        /// </summary>
        /// <param name="numberAsString">The number as string.</param>
        /// <returns>True or False</returns>
        private static bool ValidateNumber(string numberAsString)
        {
            bool isDecimal = false;
            if (string.IsNullOrEmpty(numberAsString) || string.IsNullOrWhiteSpace(numberAsString))
            {
                return false;
            }

            int parseint = 0;
            decimal parseDecimal = 0.0M;
            if (decimal.TryParse(numberAsString, out parseDecimal))
            {
                isDecimal = true;
            }
            else
            {
                if (int.TryParse(numberAsString, out parseint))
                {

                }
                else
                {
                    return false;
                }
            }

            if (numberAsString.Split('.').Length > 2 || numberAsString.Split('-').Length > 2)
            {
                return false;
            }

            if (!isDecimal && numberAsString.IndexOf('-') > -1)
            {
                if (FindSumOfDigits(Convert.ToInt32(numberAsString)) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the sum of digits.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>The sum of digits</returns>
        private static int FindSumOfDigits(int number)
        {
            if (number < 10)
            {
                return number;
            }

            return FindSumOfDigits(number / 10) + number % 10;
        }

        /// <summary>
        /// Converts the currency to words.
        /// </summary>
        /// <param name="numberAsString">The number as string.</param>
        /// <returns>The currency in words</returns>
        private static string ConvertCurrencyToWords(string numberAsString)
        {
            //defining local variables 
            string wholeNumberPart = numberAsString, decimalNumbersPart = null, andPrefix = null, convertedDecimalNumberWord = null;
            string suffix = "ONLY";

            string dollarsPrefix = "DOLLARS";
            string centsPrefix = null;

            //to locate the decimal position in given number
            int decimalPlace = numberAsString.IndexOf(".");

            //verifying decimal position
            if (decimalPlace > 0)
            {
                wholeNumberPart = numberAsString.Substring(0, decimalPlace);
                decimalNumbersPart = numberAsString.Substring(decimalPlace + 1);
                if (Convert.ToInt32(decimalNumbersPart) > 0)
                {
                    andPrefix = string.Concat(dollarsPrefix, " AND ");// separate whole numbers from points/cents 
                    centsPrefix = "CENTS";
                    suffix = string.Concat(centsPrefix, " ", suffix);
                    convertedDecimalNumberWord = ConvertNumberToWords(decimalNumbersPart);
                }
            }
            else if (decimalPlace < 0)
            {
                suffix = string.Concat(dollarsPrefix, " ", suffix);
            }

            return string.Format($"{ConvertNumberToWords(wholeNumberPart).Trim()} {andPrefix}{convertedDecimalNumberWord} {suffix}");
        }

        /// <summary>
        /// Converts the number to words.
        /// </summary>
        /// <param name="numberAsString">The number as string.</param>
        /// <returns>Number in words</returns>
        private static string ConvertNumberToWords(string numberAsString)
        {
            string numberAsWord = null;
            bool trailingZero = false;
            bool isDone = false;
            double number = Convert.ToDouble(numberAsString);

            //if ((number > 0) && number.StartsWith("0"))
            if (number > 0)
            {
                //test for zero or digit zero in a nuemric
                trailingZero = numberAsString.StartsWith("0");

                int numDigits = numberAsString.Length;
                int pos = 0;//store digit grouping
                string place = null;//digit grouping name:hundres,thousand,etc...
                switch (numDigits)
                {
                    case 1://ones' range
                        numberAsWord = Ones(numberAsString);
                        isDone = true;
                        break;
                    case 2://tens' range
                        numberAsWord = Tens(numberAsString);
                        isDone = true;
                        break;
                    case 3://hundreds' range
                        pos = (numDigits % 3) + 1;
                        place = " HUNDRED AND ";
                        break;
                    case 4://thousands' range
                    case 5:
                    case 6:
                        pos = (numDigits % 4) + 1;
                        place = " THOUSAND AND ";
                        break;
                    case 7://millions' range
                    case 8:
                    case 9:
                        pos = (numDigits % 7) + 1;
                        place = " MILLION AND ";
                        break;
                    case 10://Billions's range
                    case 11:
                    case 12:
                        pos = (numDigits % 10) + 1;
                        place = " BILLION AND ";
                        break;
                    //add extra case options for anything above Billion...
                    default:
                        isDone = true;
                        break;
                }

                if (!isDone)
                {
                    //if transalation is not done, continue...(Recursion!)
                    if (numberAsString.Substring(0, pos) != "0" && numberAsString.Substring(pos) != "0")
                    {
                        numberAsWord = ConvertNumberToWords(numberAsString.Substring(0, pos)) + place + ConvertNumberToWords(numberAsString.Substring(pos));
                    }
                    else
                    {
                        numberAsWord = ConvertNumberToWords(numberAsString.Substring(0, pos)) + ConvertNumberToWords(numberAsString.Substring(pos));
                    }

                    //check for trailing zeros
                    //if (beginsZero) word = " and " + word.Trim();
                }

                //ignore digit grouping names
                if (place != null && numberAsWord.Trim().Equals(place.Trim()))
                {
                    numberAsWord = null;
                }
            }

            return numberAsWord.Trim();
        }

        /// <summary>
        /// Tenses the specified number as string.
        /// </summary>
        /// <param name="numberAsString">The number as string.</param>
        /// <returns>Specified number as string</returns>
        private static string Tens(string numberAsString)
        {
            string numberAsWord = null;
            int number = Convert.ToInt32(numberAsString);
            switch (number)
            {
                case 10:
                    numberAsWord = "TEN";
                    break;
                case 11:
                    numberAsWord = "ELEVEN";
                    break;
                case 12:
                    numberAsWord = "TWELVE";
                    break;
                case 13:
                    numberAsWord = "THIRTEEN";
                    break;
                case 14:
                    numberAsWord = "FOURTEEN";
                    break;
                case 15:
                    numberAsWord = "FIFTEEN";
                    break;
                case 16:
                    numberAsWord = "SIXTEEN";
                    break;
                case 17:
                    numberAsWord = "SEVENTEEN";
                    break;
                case 18:
                    numberAsWord = "EIGHTEEN";
                    break;
                case 19:
                    numberAsWord = "NINETEEN";
                    break;
                case 20:
                    numberAsWord = "TWENTY";
                    break;
                case 30:
                    numberAsWord = "THIRTY";
                    break;
                case 40:
                    numberAsWord = "FOURTY";
                    break;
                case 50:
                    numberAsWord = "FIFTY";
                    break;
                case 60:
                    numberAsWord = "SIXTY";
                    break;
                case 70:
                    numberAsWord = "SEVENTY";
                    break;
                case 80:
                    numberAsWord = "EIGHTY";
                    break;
                case 90:
                    numberAsWord = "NINETY";
                    break;
                default:
                    if (number > 0)
                    {
                        numberAsWord = Tens(numberAsString.Substring(0, 1) + "0") + " " + Ones(numberAsString.Substring(1));
                    }
                    break;
            }

            return numberAsWord;
        }

        /// <summary>
        /// Oneses the specified number as string.
        /// </summary>
        /// <param name="numberAsString">The number as string.</param>
        /// <returns>Specified number as string</returns>
        private static string Ones(string numberAsString)
        {
            string numberAsWord = null;
            int number = Convert.ToInt32(numberAsString);
            switch (number)
            {
                case 1:
                    numberAsWord = "ONE";
                    break;
                case 2:
                    numberAsWord = "TWO";
                    break;
                case 3:
                    numberAsWord = "THREE";
                    break;
                case 4:
                    numberAsWord = "FOUR";
                    break;
                case 5:
                    numberAsWord = "FIVE";
                    break;
                case 6:
                    numberAsWord = "SIX";
                    break;
                case 7:
                    numberAsWord = "SEVEN";
                    break;
                case 8:
                    numberAsWord = "EIGHT";
                    break;
                case 9:
                    numberAsWord = "NINE";
                    break;
            }

            return numberAsWord;
        }
    }
}
