using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dev2.Common.Interfaces
{
    public interface IEnvironmentViewModel : IExplorerTreeItem
    {
      
        string DisplayName { get; set; }
        bool IsConnected { get; }
        bool IsLoaded { get; }
        Task<bool> Connect();
        bool IsConnecting { get; set; }

        Task<bool> Load(bool b=false);
        Task<bool> LoadDialog(string selectedId,bool b=false);
        void Filter(string filter);
        void Filter(Func<IExplorerItemViewModel, bool> filter);
        ICollection<IExplorerItemViewModel> AsList();
        void SetItemCheckedState(Guid id, bool state);
        void RemoveItem(IExplorerItemViewModel vm);
        ICommand RefreshCommand { get; set; }
        bool IsServerIconVisible { get; set; }
        bool IsServerUnavailableIconVisible { get; set; }
        ICommand ShowServerVersionCommand { get; set; }

        bool IsVisible { get; set; }
        Action SelectAll { get; set; }

        void SetPropertiesForDialog();

        void SelectItem(string selectedPath, Action<IExplorerItemViewModel> foundAction);

        Task<bool> LoadDialog(Guid selectedPath);
    }
}