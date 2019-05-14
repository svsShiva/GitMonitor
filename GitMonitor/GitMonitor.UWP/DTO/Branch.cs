using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GitMonitor.UWP.DTO
{
    public class Branch : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _enableEmailNotification;
        private bool _autoPull;
        private bool _enableDesktopNotification;

        public Branch()
        {
            Repo = new Repo();
        }

        public long BranchID { get; set; }
        public string Name { get; set; }
        public long RepoID { get; set; }
        public bool EnableDesktopNotification
        {
            get
            {
                return _enableDesktopNotification;
            }
            set
            {
                if (value != _enableDesktopNotification)
                {
                    _enableDesktopNotification = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool EnableEmailNotification
        {
            get
            {
                return _enableEmailNotification;
            }
            set
            {
                if (value != _enableEmailNotification)
                {
                    _enableEmailNotification = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool AutoPull
        {
            get
            {
                return _autoPull;
            }
            set
            {
                if (value != _autoPull)
                {
                    _autoPull = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsActive { get; set; }
        public bool HasUpstream { get; set; }
        public string Remote { get; set; }
        public string TrackingBranch { get; set; }
        public long AheadBy { get; set; }
        public long BehindBy { get; set; }

        public virtual Repo Repo { get; set; }
    }
}