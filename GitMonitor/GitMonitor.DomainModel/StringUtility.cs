namespace GitMonitor.DomainModel
{
    public static class StringUtility
    {
        public static string _unexpectedError = @"OOPS, Something went wrong!";

        public static string _nameExists = @"Name already exists";

        public static string _branchAheadByMessage = @"Your {0} branch is {1} commits behind by upstream";

        public static string _branchBehindByMessage = @"Your {0} branch is {1} commits ahead of upstream";

        public static string _branchDivergedMessage = @"Your {0} branch is {1} commits ahead of and {2} commits behind by upstream";

        public static string _emailTableHeader = @"<table style='border-collapse:collapse;border: 1px solid black;'>
                                                    <tr style='border:1px solid black;background-color:#585490;color:white'>
                                                        <td style='border:1px solid black;'>Branch Name</td>
                                                        <td style='border:1px solid black;'>Ahead By</td>  
                                                        <td style='border:1px solid black;'>Behind By</td>
                                                    </tr>";

        public static string _emailTableBody = @"<tr style='border:1px solid black;'>
                                                    <td style='border:1px solid black;'>{0}</td> 
                                                    <td style='border:1px solid black;'>{1}</td> 
                                                    <td style='border:1px solid black;'>{2}</td>
                                                </tr>";

        public static string _emailTableFooter = @"</table>";

        public static string _notificationSubject = @"GitMonitor: {0} Updates";
    }
}
