using GitMonitor.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = GitMonitor.DomainModel.DTO;

namespace GitMonitor.Repository
{
    public class EmailGroupRepository
    {
        public List<DM.EmailGroup> GetAllEmailGroups()
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    return db.Table<tblEmailGroup>()
                             .Select((m) => new DM.EmailGroup
                             {
                                 EmailGroupID = m.tblEmailGroupID,
                                 Name = m.Name,
                                 Emails = m.Emails,
                                 CreatedAt = m.CreatedAt,
                                 LastModifiedAt = m.LastModifiedAt
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

        public void Add(DM.EmailGroup emailGroup)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    var rec = db.Table<tblEmailGroup>()
                                .Where(m => m.Name == emailGroup.Name)
                                .FirstOrDefault();

                    if (rec != null)
                    {
                        //TODO custom exception handling
                        //Control will return
                    }

                    db.Insert(new tblEmailGroup
                    {
                        Name = emailGroup.Name,
                        Emails = emailGroup.Emails,
                        LastModifiedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void Delete(long id)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    db.Table<tblEmailGroup>().Delete(m => m.tblEmailGroupID == id);
                }
            }
            catch (Exception ex)
            {
                ErrorRepo.AddErrorLog(exception: ex);
                throw;
            }
        }

        public void Update(DM.EmailGroup emailGroup)
        {
            try
            {
                using (SQLiteConnection db = InitializeDB.GetSQLiteConnection())
                {
                    tblEmailGroup tblEmailGroup = db.Table<tblEmailGroup>()
                                                    .FirstOrDefault(m => m.tblEmailGroupID == emailGroup.EmailGroupID);

                    tblEmailGroup.Name = emailGroup.Name;
                    tblEmailGroup.Emails = emailGroup.Emails;
                    tblEmailGroup.LastModifiedAt = DateTime.Now;

                    db.Update(tblEmailGroup);
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
