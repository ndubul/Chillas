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
        bool IsConnecting { get; }
        Task<bool> Load();
        Task<bool> LoadDialog(Guid? selectedId);
        void Filter(string filter);
        ICollection<IExplorerItemViewModel> AsList();
        void SetItemCheckedState(Guid id, bool state);
        void RemoveItem(IExplorerItemViewModel vm);
        ICommand RefreshCommand { get; set; }
        bool IsServerIconVisible { get; set; }
        bool IsServerUnavailableIconVisible { get; set; }
        ICommand ShowServerVersionCommand { get; set; }
        Action<IExplorerItemViewModel> SelectAction { get; set; }

        void SetPropertiesForDialog();

        void SelectItem(string selectedPath, Action<IExplorerItemViewModel> foundAction);
    }
}