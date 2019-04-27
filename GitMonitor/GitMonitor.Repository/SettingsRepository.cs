using GitMonitor.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    public class SettingsRepository
    {
        public List<DM.Setting> GetAllSettings()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblSetting>()
                             .Where(m => m.IsActive == true)
                             .Select((m) => new DM.Setting
                             {
                                 SettingID = m.tblSettingID,
                                 Key = m.Key,
                                 Value = m.Value,
                                 IsActive = m.IsActive
                             }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }
    }
}