namespace GitMonitor.UWP.Utilities
{
    internal static class RouteUtility
    {
        //TODO get base url or port from sqliteDB
        internal static string _baseRoute = @"http://localhost:8003/API/";

        internal static string _getGetAllTrackedRepos = _baseRoute + @"/Repo/GetAllTrackedRepos";
        internal static string _getGetAllUnTrackedRepos = _baseRoute + @"/Repo/GetAllUnTrackedRepos";
        internal static string _getGetRepoByID = _baseRoute + @"/Repo/GetRepoByID?id={0}";
        internal static string _getUpdateRepo = _baseRoute + @"/Repo/UpdateRepo";
        internal static string _getStopTrackingRepo = _baseRoute + @"/Repo/StopTrackingRepo?id={0}";
        internal static string _getRefreshRepo = _baseRoute + @"/Repo/RefreshRepo?id={0}";

        internal static string _getGetAllEmailGroups = _baseRoute + @"/EmailGroup/GetAllEmailGroups";
        internal static string _getUpdateEmailGroup = _baseRoute + @"/EmailGroup/UpdateEmailGroup";
        internal static string _getAddEmailGroup = _baseRoute + @"/EmailGroup/AddEmailGroup";
        internal static string _getDeleteEmailGroup = _baseRoute + @"/EmailGroup/DeleteEmailGroup?id={0}";
    }
}