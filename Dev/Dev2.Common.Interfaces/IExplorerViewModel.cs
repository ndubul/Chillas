using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Dev2.Common.Interfaces
{

    public delegate void SelectedExplorerEnvironmentChanged(object sender, IEnvironmentViewModel e);

	public interface IExplorerViewModel:INotifyPropertyChanged
	{
		ICollection<IEnvironmentViewModel> Environments {get;set;}
        void Filter(string filter);
        void RemoveItem(IExplorerItemViewModel item);
        event SelectedExplorerEnvironmentChanged SelectedEnvironmentChanged;
        IEnvironmentViewModel SelectedEnvironment { get; set; }
        IServer SelectedServer { get;  }
	    IConnectControlViewModel ConnectControlViewModel { get; }
        string SearchText { get; set; }
	    ICommand RefreshCommand { get; set; }
        bool IsRefreshing { get; set; }
	    bool ShowConnectControl { get; set; }
	    IExplorerTreeItem SelectedItem { get; set; }
	    object[] SelectedDataItems { get; set; }
	    ICommand ClearSearchTextCommand { get; }
	    ICommand CreateFolderCommand { get; }

	    void SelectItem(Guid id);
	    IList<IExplorerItemViewModel> FindItems(Func<IExplorerItemViewModel, bool> filterFunc);
	}
}