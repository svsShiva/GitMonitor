namespace GitMonitor.UWP.Utilities
{
    internal static class StringUtility
    {
        internal static string _emailRegularExpression = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@"
                                                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        internal static string _unexpectedError = @"OOPS, Something went wrong!";

        internal static string _repoTrackedSucessfully = @"Started tracking the selected repository {0}";

        internal static string _repoUntrackedSucessfully = @"Untracking the selected repository {0}";

        internal static string _repoUnTrackedConfirmation = @"Untrack the selected repository {0} ?";

        internal static string _invalidEmail = @"Invalid Email";

        internal static string _emailAlreadyAdded = @"Email already exists";

        internal static string _emptyGroupName = @"Name cannot not be empty";

        internal static string _emptyEmail = @"Email cannot not be empty";

        internal static string _lengthMaxGroupName = @"length cannot be more than 20";

        internal static string _nameExists = @"Name already exists";
    }
}
