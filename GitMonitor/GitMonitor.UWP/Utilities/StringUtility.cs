namespace GitMonitor.UWP.Utilities
{
    internal static class StringUtility
    {
        internal static string _emailRegularExpression = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@"
                                                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        internal static string _numberRegularExpression = @"^[0-9]*$";


        internal static string _unexpectedError = @"OOPS, Something went wrong!";

        internal static string _repoTrackedSucessfully = @"Started tracking the selected repository {0}";

        internal static string _repoUntrackedSucessfully = @"Untracking the selected repository {0}";

        internal static string _repoUnTrackedConfirmation = @"Untrack the selected repository {0} ?";

        internal static string _invalidEmail = @"Invalid Email";

        internal static string _emailAlreadyAdded = @"Email already exists";

        internal static string _lengthMaxGroupName = @"length cannot be more than 20";

        internal static string _nameExists = @"Name already exists";

        internal static string _emailGroupDeletionConfirmation = @"Delete the selected Email Group {0} ?";

        internal static string _emailGroupDeletedSucessfully = @"Deleted the Email Group {0} ?";

        internal static string _invalidInterval = @"Interval can only take numbers";

        internal static string _saveSettingConfirmation = @"Do you want to save the changes ?";

        internal static string _settingSavedSuccessfully = @"Changes saved successfully";

        internal static string _InvalidRange = @"Value shoud be between {0} and {1}";

        internal static string _emptyField = @"This field cannot not be empty";
    }
}
