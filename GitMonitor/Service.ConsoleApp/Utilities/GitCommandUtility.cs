namespace Service.ConsoleApp.Utilities
{
    class GitCommandUtility
    {
        public static string Git { get { return "git {0}"; } }
        public static string GitFetch { get { return string.Format(Git, "fetch"); } }
    }
}