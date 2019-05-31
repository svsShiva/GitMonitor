using SQLite;
using System.IO;
using GitMonitor.DomainModel.Enums;
using System.Linq;

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
                    if (db.Table<tblSetting>().Count() == 0)
                    {
                        db.Insert(new tblSetting { Key = SettingEnum.Interval.ToString(), Value = "5" });
                        db.Insert(new tblSetting { Key = SettingEnum.EnableDesktopNotifications.ToString(), Value = "True" });
                        db.Insert(new tblSetting { Key = SettingEnum.EnableEmailNotifications.ToString(), Value = "False" });
                        db.Insert(new tblSetting { Key = SettingEnum.MinInterval.ToString(), Value = "5" });
                        db.Insert(new tblSetting { Key = SettingEnum.MaxInterval.ToString(), Value = "60" });
                        db.Insert(new tblSetting { Key = SettingEnum.SMTPEmail.ToString(), Value = "" });
                        db.Insert(new tblSetting { Key = SettingEnum.SMTPPassword.ToString(), Value = "" });
                        db.Insert(new tblSetting { Key = SettingEnum.SMTPHost.ToString(), Value = "" });
                        db.Insert(new tblSetting { Key = SettingEnum.SMTPPort.ToString(), Value = "" });
                        db.Insert(new tblSetting { Key = SettingEnum.SMTPEnableSsl.ToString(), Value = "False" });
                        db.Insert(new tblSetting { Key = SettingEnum.EnableLog.ToString(), Value = "True" });
                        db.Insert(new tblSetting { Key = SettingEnum.SimultaneousCheckCount.ToString(), Value = "10" });
                        db.Insert(new tblSetting { Key = SettingEnum.LastModifiedRunInterval.ToString(), Value = "10" });
                        db.Insert(new tblSetting { Key = SettingEnum.LogPath.ToString(), Value = @"C:\" });
                    }
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
