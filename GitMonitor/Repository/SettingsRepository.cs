using DataModel;
using System.Collections.Generic;
using System.Linq;
using DM = DomainModel.DTO;

namespace Repository
{
   public class SettingsRepository
    {
        public List<DM.Settings> GetAllSettings()
        {
            return DataSource._settings
                .Where(m => m.IsActive = true)
                .Select((m) => new DM.Settings
                {
                    SettingsID = m.tblSettingsID,
                    Key = m.Key,
                    Value = m.Value,
                    IsActive = m.IsActive
                }
                )
                .ToList();
        }
    }
}
