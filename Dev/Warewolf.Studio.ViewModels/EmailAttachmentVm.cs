
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        IEmailAttachmentModel _model;
        public MessageBoxResult Result { get; private set; }

        public EmailAttachmentVm(IList<string> attachments, IEmailAttachmentModel model, Action closeAction)
            : this(model, closeAction)
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
            Drives = model.FetchDrives().Select(a => new FileListingModel(model, a, () => OnPropertyChanged("DriveName"))).ToList();
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }

        public IList<FileListingModel> Drives { get; set; }

        void Expand(IEnumerable<string> attachments)
        {
            foreach (var attachment in attachments)
            {
                SelectAttachment(attachment, Drives);
            }
        }

        public void SelectAttachment(string name, IEnumerable<IFileListingModel> model)
        {
            if (name.Contains("\\"))
            {
                string node = name.Contains(":") ? name.Substring(0, name.IndexOf("\\", StringComparison.Ordinal) + 1) : name.Substring(0, name.IndexOf("\\", StringComparison.Ordinal));
                var toExpand = model.FirstOrDefault(a => a.Name == node);
                if (toExpand != null)
                {
                    toExpand.IsExpanded = true;
                    SelectAttachment(name.Substring(name.IndexOf("\\", StringComparison.Ordinal) + 1), toExpand.Children);
                }
            }
            else
            {
                var toExpand = model.FirstOrDefault(a => a.Name == name);
                if (toExpand != null)
                {
                    toExpand.IsChecked = true;
                }
            }
        }

        public void Cancel()
        {
            Result = MessageBoxResult.Cancel;
            _closeAction();
        }

        public void Save()
        {
            Result = MessageBoxResult.OK;
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
                return String.Join(";", Drives.SelectMany(a => a.FilterSelected(new List<string>())).ToList());
                // return _driveName;
            }
            set
            {
                _driveName = value;
                OnPropertyChanged(() => DriveName);
            }
        }

        public List<string> GetAttachments()
        {
            return Drives.SelectMany(a => a.FilterSelected(new List<string>())).ToList();
        }

        void SetIsChecked(bool value)
        {
            if (value)
            {

            }
        }
    }

}