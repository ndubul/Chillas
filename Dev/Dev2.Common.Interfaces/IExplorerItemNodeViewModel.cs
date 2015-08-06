using System.Collections.Generic;
using System.Windows;

namespace Dev2.Common.Interfaces
{
    public interface IExplorerItemNodeViewModel : IExplorerItemViewModel
    {
        IExplorerItemNodeViewModel Self { get; set; }
        int Weight { get; set; }
        ICollection<IExplorerItemNodeViewModel> AsList();
        Visibility IsMainNode { get; set; }
        Visibility IsNotMainNode { get; set; }
    }
}
