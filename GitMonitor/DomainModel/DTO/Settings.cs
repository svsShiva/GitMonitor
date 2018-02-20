namespace DomainModel.DTO
{
    public partial class Settings
    {
        public long SettingsID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
