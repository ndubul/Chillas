using System.Collections.Generic;

namespace Dev2.Common.Interfaces
{
    public interface IEmailAttachmentView
    {
        void ShowView(IEnumerable<string> current);
    }
}