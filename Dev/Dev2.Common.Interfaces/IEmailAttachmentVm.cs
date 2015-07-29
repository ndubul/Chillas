using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace Dev2.Common.Interfaces
{
    public interface IEmailAttachmentVm
    {


        void Cancel();

        void Save();

        DelegateCommand CancelCommand { get; set; }
        DelegateCommand SaveCommand { get; set; }
        IList<string> Attachments { get; set; }
        string DriveName { get; set; }
        bool IsChecked { get; set; }


        List<string> GetAttachments();
    }
}