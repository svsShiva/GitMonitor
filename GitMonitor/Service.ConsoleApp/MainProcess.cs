using Repository;
using System.Threading.Tasks;
using DomainModel.DTO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Configuration;
using System.Timers;
using Service.ConsoleApp.Utilities;

namespace Service.ConsoleApp
{
    class MonitoringProcess
    {
        int _simultaneousCheckCount;
        Timer _timer;
        bool _isProcessExecuting;
        List<Setting> _settings;

        public MonitoringProcess()
        {
            try
            {
                _isProcessExecuting = false;

                _simultaneousCheckCount = Convert.ToInt16(ConfigurationManager.AppSettings["SimultaneousCheckCount"].ToString());

                _timer = new Timer();
                _timer.Enabled = true;
                _timer.Interval = TimeSpan
                                  .FromMinutes(Convert.ToInt16(ConfigurationManager.AppSettings["RunInterval"].ToString()))
                                  .TotalMilliseconds;

                _timer.Elapsed += Timer_Elapsed;
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        public void StartProcess()
        {
            try
            {
                Timer_Elapsed(this, null);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isProcessExecuting)
                {
                    _settings = new SettingsRepository().GetAllSettings();

                    _timer.Interval = TimeSpan
                                          .FromMinutes(Convert.ToInt16(
                                                       _settings.Where(m => m.Key == "Interval")
                                          .FirstOrDefault().Value)
                                                       )
                                          .TotalMilliseconds;

                    _isProcessExecuting = true;

                    RepoRepository repoRepository = new RepoRepository();
                    List<Repo> list = repoRepository.GetAllRepos();

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        CheckRepoStatus(list.Skip(_simultaneousCheckCount * i)
                                            .Take(_simultaneousCheckCount).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }

            _isProcessExecuting = false;
        }

        private void CheckRepoStatus(List<Repo> items)
        {
            try
            {
                // need to test this logic for multiple calls
                Action[] actionList = new Action[items.Count];

                int count = 0;
                foreach (var item in items)
                {
                    actionList[count] = new Action(() => RunTasks(item));
                    count++;
                }

                // this will run the actions in parallel and wait till all the actions gets completed
                Parallel.Invoke(actionList);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void RunTasks(Repo repo)
        {
            try
            {
                //More methods will be added here;
                FetchCommand(repo);
                UpdateCurrentBranch();
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void FetchCommand(Repo repo)
        {
            try
            {
                ProcessUtility.ExecuteCommand(repo.WorkingDirectory, GitCommandUtility.GitFetch);
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        private void UpdateCurrentBranch()
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }
    }
}