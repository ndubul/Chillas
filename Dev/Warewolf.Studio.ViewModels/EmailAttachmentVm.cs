using System;
using System.Collections.Generic;
using System.Linq;
using Dev2.Common.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Studio.Core;

namespace Warewolf.Studio.ViewModels
{
    public class EmailAttachmentVm : BindableBase, IEmailAttachmentVm
    {
        IList<string> _attachments;
        readonly Action _closeAction;
        DelegateCommand _saveCommand;
        DelegateCommand _cancelCommand;

        string _driveName;
        bool _isChecked;
        IEmailAttachmentModel _model;
        readonly IList<FileListingModel> _drives;

        public EmailAttachmentVm(IList<string> attachments, IEmailAttachmentModel model, Action closeAction) : this(model, closeAction)
        {
            _attachments = attachments;
            Expand(attachments);
            _model = model;
           
        }
        public EmailAttachmentVm(IEmailAttachmentModel model, Action closeAction)
        {
            _closeAction = closeAction;
            _attachments = new List<string>();
            _model = model;
            _drives = model.FetchDrives().Select(a => new FileListingModel(model, a)).ToList();
        }


        void Expand(IEnumerable<string> attachments)
        {

            foreach(var attachment in attachments)
            {
                SelectAttachment(attachment,_drives);
            }
            
        }

        public void SelectAttachment(string name, IEnumerable<FileListingModel> model )
        {
            if (name.Contains("\\"))
            {
                string node = name.Substring(0, name.IndexOf("\\", StringComparison.Ordinal) );
                var toExpand = model.FirstOrDefault(a => a.Name == node);
                if(toExpand != null)
                {
                    toExpand.IsExpanded = true;
                }
            }
            else
            {
                var toExpand = model.FirstOrDefault(a => a.Name == name);
                if (toExpand != null)
                {
                    toExpand.IsSelected = true;
                }
            }
        }

        public void Cancel()
        {
            _closeAction();
        }

        public void Save()
        {
            _closeAction();
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

        public IList<string> Attachments
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

        public List<string> GetAttachments()
        {
            return _drives.SelectMany(a=>a.FilterSelected(new List<string>())).ToList();
        }

        void SetIsChecked(bool value)
        {
            if (value)
            {

            }
        }
    }

}