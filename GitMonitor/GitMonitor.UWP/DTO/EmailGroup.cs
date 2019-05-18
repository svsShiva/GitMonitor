using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GitMonitor.UWP.DTO
{
    public class EmailGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private long _emailGroupID;
        private string _name = string.Empty;
        private string _emails = string.Empty;
        private DateTime _createdAt;
        private bool _isActive;
        private DateTime _lastModifiedAt;

        //For UI
        private bool _isSelected;

        public DateTime LastModifiedAt
        {
            get
            {
                return _lastModifiedAt;
            }
            set
            {
                if (value != _lastModifiedAt)
                {
                    _lastModifiedAt = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public long EmailGroupID
        {
            get
            {
                return _emailGroupID;
            }
            set
            {
                if (value != _emailGroupID)
                {
                    _emailGroupID = value;
                }
            }
        }
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
        public string Emails
        {
            get
            {
                return _emails;
            }
            set
            {
                if (value != _emails)
                {
                    _emails = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                if (value != _createdAt)
                {
                    _createdAt = value;
                }
            }
        }
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}