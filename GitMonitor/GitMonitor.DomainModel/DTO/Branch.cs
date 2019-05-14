namespace GitMonitor.DomainModel.DTO
{
    public class Branch
    {
        public Branch()
        {
            Repo = new Repo();
        }

        public long BranchID { get; set; }
        public string Name { get; set; }
        public long RepoID { get; set; }
        public bool EnableDesktopNotification { get; set; }
        public bool EnableEmailNotification { get; set; }
        public bool AutoPull { get; set; }
        public bool IsActive { get; set; }
        public bool HasUpstream { get; set; }
        public string Remote { get; set; }
        public string TrackingBranch { get; set; }
        public long AheadBy { get; set; }
        public long BehindBy { get; set; }

        public virtual Repo Repo { get; set; }
    }
}