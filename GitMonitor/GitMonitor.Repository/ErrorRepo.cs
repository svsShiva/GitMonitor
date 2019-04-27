using GitMonitor.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    public class ErrorRepo
    {
        public List<DM.ErrorLog> GetErrorLog()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblErrorLog>()
                        .Select((m) => new DM.ErrorLog
                        {
                            ErrorLogID = m.tblErrorLogID,
                            Description = m.Description,
                            LogTime = m.LogTime
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                AddErrorLog(exception: ex);
                throw;
            }
        }

        public static void AddErrorLog(DM.ErrorLog errorLog = null, Exception exception = null)
        {
            tblErrorLog tblErrorLog = new tblErrorLog();
            tblErrorLog.LogTime = DateTime.Now;
            tblErrorLog.Description = errorLog != null ?
                                      errorLog.Description :
                                      exception.InnerException +
                                      exception.StackTrace +
                                      exception.InnerException != null ? exception.InnerException.Message : string.Empty;

            using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
            {
                db.Insert(tblErrorLog);
            }
        }

        public static void AddErrorLog(List<DM.ErrorLog> errorLogs = null, List<Exception> exceptions = null)
        {
            using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
            {
                List<tblErrorLog> list = new List<tblErrorLog>();

                if (errorLogs != null)
                {
                    list = errorLogs
                           .Select(m => new tblErrorLog
                           {
                               Description = m.Description,
                               LogTime = DateTime.Now
                           })
                           .ToList();
                }
                else
                {
                    list = exceptions
                           .Select(m => new tblErrorLog
                           {
                               Description = m.Message +
                                             m.StackTrace +
                                             m.InnerException != null ? m.InnerException.Message : string.Empty,
                               LogTime = DateTime.Now
                           })
                           .ToList();
                }

                db.InsertAll(list);
            }
        }
    }
}