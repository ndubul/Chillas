using System.Collections.Generic;

namespace Dev2.Common.Interfaces
{
    public interface IExplorerItemNodeViewModel : IExplorerItemViewModel
    {
        IExplorerItemNodeViewModel Self { get; set; }
        int Weight { get; set; }
        ICollection<IExplorerItemNodeViewModel> AsList();
        bool IsMainNode { get; set; }
        bool IsNotMainNode { get; set; }
    }
}
