using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class EmailAttachmentVm : BindableBase
    {
        IEnumerable<string> _attachments;
        readonly Action _action;
        DelegateCommand _saveCommand;
        DelegateCommand _cancelCommand;
        Listing _selectedDrive;
        string _driveName;
        bool _isChecked;

        public EmailAttachmentVm(IEnumerable<string> attachments, Action action)
        {
            _attachments = attachments;
            _action = action;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            Drives = new ObservableCollection<Drive>();

            Drive drive = new Drive();
            drive.Children = new ObservableCollection<Listing>();

            drive.Title = "C:";
            Listing listing = new Listing();
            listing.Name = "AppData";
            drive.Children.Add(listing);

            //drive.Title = "D:";
            //listing.Name = "Projects";
            //drive.Children.Add(listing);

            Drives.Add(drive);
        }

        public ObservableCollection<Drive> Drives { get; set; }

        void Cancel()
        {
            _action();
        }

        void Save()
        {
            _action();
        }

        public DelegateCommand CancelCommand
        {
            get
            {
                return _cancelCommand;
            }
            set
            {
                _cancelCommand = value;
            }
        }

        public DelegateCommand SaveCommand
        {
            get
            {
                return _saveCommand;
            }
            set
            {
                _saveCommand = value;
            }
        }

        public IEnumerable<string> Attachments
        {
            get
            {
                return _attachments;
            }
            set
            {
                _attachments = value;
            }
        }

        public Listing SelectedDrive
        {
            get
            {
                return _selectedDrive;
            }
            set
            {
                if (value == null) return;
                _selectedDrive = value;
                OnPropertyChanged(() => SelectedDrive);
                if (SelectedDrive != null)
                {
                    DriveName = SelectedDrive.Name + "; ";
                }
            }
        }

        public string DriveName
        {
            get
            {
                return _driveName;
            }
            set
            {
                _driveName = value;
                OnPropertyChanged(() => DriveName);
            }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                SetIsChecked(value);
            }
        }

        void SetIsChecked(bool value)
        {
            if (value)
            {

            }
        }
    }

    public class Drive
    {
        public string Title { get; set; }
        public ObservableCollection<Listing> Children { get; set; }
    }

    public class Listing
    {
        public string Name { get; set; }
    }
}