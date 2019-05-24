using SQLite;

namespace GitMonitor.DataModel
{
    [Table("Setting")]
    public class tblSetting
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public long tblSettingID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}