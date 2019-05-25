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

        public void Update(List<DM.Setting> settings)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    foreach (var item in settings)
                    {
                        tblSetting tblSetting = db.Table<tblSetting>()
                                                        .FirstOrDefault(m => m.Key == item.Key);

                        if (tblSetting != null)
                        {
                            tblSetting.Value = item.Value;

                            db.Update(tblSetting);
                        }
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