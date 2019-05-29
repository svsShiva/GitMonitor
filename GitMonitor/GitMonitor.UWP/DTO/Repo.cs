using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GitMonitor.UWP.DTO
{
    public class Repo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        private bool _enableDesktopNotification;
        private bool _enableEmailNotification;

        public Repo()
        {
            this.Branches = new List<Branch>();
        }

        public long RepoID { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string RepoUrl { get; set; }
        public string WorkingDirectory { get; set; }
        public bool AutoTrack { get; set; }
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
        public string EmailGroupIDS { get; set; }
        public string CurrentBranch { get; set; }
        public bool IsUntrackedRepo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? RecentCheck { get; set; }
        public bool IsActive { get; set; }
        public bool IsAhead { get; set; }
        public bool IsBehind { get; set; }
        public bool IsUptoDate { get; set; }

        public virtual List<Branch> Branches { get; set; }
    }
}
