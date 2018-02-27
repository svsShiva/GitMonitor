using DomainModel.DTO;
using System;
using Service.ConsoleApp.Utilities;

namespace Service.ConsoleApp.Utilities
{
    class GitUtility
    {
        static string Git { get { return "git {0}"; } }
        static string GitFetch { get { return string.Format(Git, "fetch"); } }

        private static void FetchCommand(Repo repo)
        {
            try
            {
                ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitFetch);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private static void UpdateCurrentBranch()
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        public static void RunTasks(Repo repo)
        {
            try
            {
                FetchCommand(repo);
                UpdateCurrentBranch();
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }
    }
}