using SQLite;
using System.IO;

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
                }
            }
            catch (System.Exception ex)
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
