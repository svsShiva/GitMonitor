using SQLite;
using System.IO;
using GitMonitor.DomainModel.Enums;

namespace GitMonitor.DataModel
{
    public static class InitializeDB
    {
        static string _conn = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)
                                          .DirectoryName
                                          .Replace(@"\bin\Debug", "")
                                          .Replace(@"\bin\Release", "")
                                          + @"\DB\GitMonitor.db";

        static InitializeDB()
        {
            try
            {
                using (SQLiteConnection db = GetSQLiteConnection())
                {
                    db.CreateTable<tblSetting>();
                    db.CreateTable<tblRepo>();
                    db.CreateTable<tblBranch>();
                    db.CreateTable<tblErrorLog>();
                    db.CreateTable<tblEmailGroup>();

                    //Seed Data
                    db.Insert(new tblSetting { Key = SettingEnum.Interval.ToString(), Value = "5" });
                    db.Insert(new tblSetting { Key = SettingEnum.EnableDesktopNotifications.ToString(), Value = "True" });
                    db.Insert(new tblSetting { Key = SettingEnum.EnableEmailNotifications.ToString(), Value = "False" });
                    db.Insert(new tblSetting { Key = SettingEnum.MinInterval.ToString(), Value = "5" });
                    db.Insert(new tblSetting { Key = SettingEnum.MaxInterval.ToString(), Value = "60" });
                }
            }
            catch
            {
                throw;
            }
        }

        public static SQLiteConnection GetSQLiteConnection()
        {
            return new SQLiteConnection(_conn);
        }
    }
}
