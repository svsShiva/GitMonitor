using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitMonitor.Repository;
using GitMonitor.DomainModel.DTO;
using System.Configuration;
using System.Timers;
using GitMonitor.DomainModel.Enums;

namespace GitMonitor.Service.ConsoleApp.Utilities
{
    class TimerUtility
    {
        int _simultaneousCheckCount;
        System.Timers.Timer _timer;
        bool _isProcessExecuting;
        List<Setting> _settings;

        public TimerUtility()
        {
            try
            {
                _isProcessExecuting = false;

                _settings = new SettingsRepository().GetAllSettings();

                _simultaneousCheckCount = Convert.ToInt16(ConfigurationManager.AppSettings["SimultaneousCheckCount"].ToString());

                _timer = new System.Timers.Timer();
                _timer.Enabled = true;
                _timer.Interval = TimeSpan
                                          .FromMinutes(Convert.ToInt16(
                                                       _settings.Where(m => m.Key == SettingEnum.Interval.ToString())
                                          .FirstOrDefault().Value)
                                                       )
                                          .TotalMilliseconds;

                _timer.Elapsed += Timer_Elapsed;
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }

        public void StartTimer()
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

        public void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isProcessExecuting)
                {
                    _timer.Interval = TimeSpan
                                          .FromMinutes(Convert.ToInt16(
                                                       _settings.Where(m => m.Key == SettingEnum.Interval.ToString())
                                          .FirstOrDefault().Value)
                                                       )
                                          .TotalMilliseconds;

                    _isProcessExecuting = true;

                    RepoRepository repoRepository = new RepoRepository();
                    List<Repo> list = repoRepository.GetAllTrackedRepos();
                    //list.AddRange(repoRepository.GetAllUnTrackedRepos());

                    for (int i = 0; i < list.Count; i++)
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

        public void CheckRepoStatus(List<Repo> items)
        {
            try
            {
                // need to test this logic for multiple calls
                Action[] actionList = new Action[items.Count];

                int count = 0;
                foreach (var item in items)
                {
                    actionList[count] = new Action(() => GitUtility.RunTasks(item, false));
                    count++;
                }

                // this will run the actions in parallel and wait till all the actions gets completed
                Parallel.Invoke(actionList);

                // Notification & Email service
                NotificationUtility.SendNotification();
            }
            catch (Exception ex)
            {
                LogUtility.LogMessage(ex);
            }
        }
    }
}
