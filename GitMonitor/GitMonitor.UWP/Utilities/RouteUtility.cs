namespace GitMonitor.UWP.Utilities
{
    internal static class RouteUtility
    {
        //TODO get base url or port from sqliteDB
        internal static string _baseRoute = @"http://localhost:8003/API/";

        internal static string _getAllTrackedRepos = _baseRoute + @"/Repo/GetAllTrackedRepos";
        internal static string _getAllUnTrackedRepos = _baseRoute + @"/Repo/GetAllUnTrackedRepos";
        internal static string _getRepoByID = _baseRoute + @"/Repo/GetRepoByID?id={0}";
        internal static string _updateRepo = _baseRoute + @"/Repo/UpdateRepo";
        internal static string _stopTrackingRepo = _baseRoute + @"/Repo/StopTrackingRepo?id={0}";
        internal static string _refreshRepo = _baseRoute + @"/Repo/RefreshRepo?id={0}";
        internal static string _syncRepo = _baseRoute + @"/Repo/SyncRepo?id={0}";

        internal static string _getAllEmailGroups = _baseRoute + @"/EmailGroup/GetAllEmailGroups";
        internal static string _updateEmailGroup = _baseRoute + @"/EmailGroup/UpdateEmailGroup";
        internal static string _addEmailGroup = _baseRoute + @"/EmailGroup/AddEmailGroup";
        internal static string _deleteEmailGroup = _baseRoute + @"/EmailGroup/DeleteEmailGroup?id={0}";

        internal static string _getAllSetting = _baseRoute + @"/Setting/GetAllSetting";
        internal static string _updateSetting = _baseRoute + @"/Setting/UpdateSetting";
    }
}