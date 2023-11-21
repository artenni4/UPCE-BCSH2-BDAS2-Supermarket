using System.Text.RegularExpressions;

namespace Supermarket.Wpf.Common
{
    public static class ValidateInput
    {
        public static bool IsValidStringInput(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            Regex regex = new Regex("^[a-zA-Z0-9ěščřžýáíéúůďťňĚŠČŘŽÝÁÍÉÚŮĎŤŇ ]*$");

            return regex.IsMatch(input);
        }

        public static bool IsValidPersonalNumber(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            input = input.Replace(" ", "");

            string format = @"^\d{6}/\d{4}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(input, format))
                return false;

            string[] parts = input.Split('/');

            if (parts.Length != 2 || !parts.All(p => p.All(char.IsDigit)))
                return false;

            int day = int.Parse(parts[0].Substring(4, 2));
            int month = int.Parse(parts[0].Substring(2, 2));
            if (month > 50 && month < 63)
                month -= 50;
            else if (month > 20 && month < 33)
                month -= 20;
            else if (month > 70 && month < 73)
                month -= 70;

            int year = int.Parse(parts[0].Substring(0, 2));

            if (year < 54)
                year += 2000;
            else
                year += 1900;

            try
            {
                DateTime dateOfBirth = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                return false;
            }

            var numbers = parts[0] + parts[1];
            int odds = int.Parse(numbers[0].ToString()) + int.Parse(numbers[2].ToString()) + int.Parse(numbers[4].ToString()) + int.Parse(numbers[6].ToString()) + int.Parse(numbers[8].ToString());
            int evens = int.Parse(numbers[1].ToString()) + int.Parse(numbers[3].ToString()) + int.Parse(numbers[5].ToString()) + int.Parse(numbers[7].ToString()) + int.Parse(numbers[9].ToString());
            int difference = odds - evens;
            if (difference % 11 != 0)
                return false;

            return true;
        }
    }
}
