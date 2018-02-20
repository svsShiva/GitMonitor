namespace DataModel
{
    public partial class tblSettings
    {
        public long tblSettingsID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
