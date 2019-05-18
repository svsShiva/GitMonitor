using System.Text.RegularExpressions;

namespace GitMonitor.UWP.Utilities
{
    internal static class ValidationUtility
    {
        internal static string ValidateName(string name)
        {
            if (name == string.Empty)
            {
                return StringUtility._emptyGroupName;
            }
            else if (name.Length > 20)
            {
                return StringUtility._lengthMaxGroupName;
            }

            return string.Empty;
        }

        internal static string ValidateEmail(string email)
        {
            if (email == string.Empty)
            {
                return StringUtility._emptyEmail;
            }
            else if (!Regex.IsMatch(email, StringUtility._emailRegularExpression))
            {
                return StringUtility._invalidEmail;
            }

            return string.Empty;
        }
    }
}
