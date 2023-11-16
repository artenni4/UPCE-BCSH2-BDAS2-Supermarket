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

            Regex regex = new Regex("^[a-zA-Z0-9ěščřžýáíéúůďťňĚŠČŘŽÝÁÍÉÚŮĎŤŇ]*$");

            return regex.IsMatch(input);
        }


    }
}
