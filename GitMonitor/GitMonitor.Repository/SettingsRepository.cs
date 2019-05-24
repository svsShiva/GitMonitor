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
                             .Select((m) => new DM.Setting
                             {
                                 SettingID = m.tblSettingID,
                                 Key = m.Key,
                                 Value = m.Value,
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

        public void Update(DM.Setting setting)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    tblSetting tblSetting = db.Table<tblSetting>()
                                                    .FirstOrDefault(m => m.Key == setting.Key);

                    if (tblSetting != null)
                    {

                        tblSetting.Key = setting.Key;
                        tblSetting.Value = setting.Value;

                        db.Update(tblSetting);
                    }
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