using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.IO;

namespace GitMonitor.DataModel
{
    internal class ConnectionBuilder
    {
        public static DbConnection GetSqlConnection()
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            // local DB folder path
            string localDBConnString = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName
                                               .Replace("\\bin\\Debug", "")
                                               .Replace("\\bin\\Release", "")
                                                + @"\\DB\\GitMonitor.db;";

            // use your ADO.NET connection string
            entityBuilder.ProviderConnectionString = @"data source=" + localDBConnString;

            entityBuilder.Provider = "System.Data.SQLite.EF6";

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/GitMonitorModel.csdl|res://*/GitMonitorModel.ssdl|res://*/GitMonitorModel.msl";

            return new EntityConnection(entityBuilder.ConnectionString);
        }
    }
}
