using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Dev2.Common.Interfaces.Data;

namespace Dev2.Common.Interfaces
{
    public interface IExplorerTreeItem
    {
        ResourceType ResourceType { get; set; }
        string ResourcePath { get; set; }
        string ResourceName { get; set; }
        Guid ResourceId { get; set; }
        bool IsExpanderVisible { get; set; }
        ICommand NewCommand { get; set; }
        ICommand DeployCommand { get; set; }
        bool CanCreateDbService { get; set; }
        bool CanCreateDbSource { get; set; }
        bool CanCreateServerSource { get; set; }
        bool CanCreateWebService { get; set; }
        bool CanCreateWebSource { get; set; }
        bool CanCreatePluginService { get; set; }
        bool CanCreatePluginSource { get; set; }
        bool CanCreateEmailSource { get; set; }
        bool CanCreateDropboxSource { get; set; }
        bool CanCreateSharePointSource { get; set; }
        bool CanRename { get; set; }
        bool CanDelete { get; set; }
        bool CanCreateFolder { get; set; }
        bool CanDeploy { get; set; }
        bool CanShowVersions { get; set; }
        bool CanRollback { get;  }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
		bool CanShowServerVersion { get; set; }
        bool AllowResourceCheck { get; set; }
        bool? IsResourceChecked { get; set; }
       
        ICommand RenameCommand { get; set; }
        ICommand CreateFolderCommand { get; set; }
        ICommand DeleteCommand { get; set; }
        ICommand ShowVersionHistory { get; set; }
        ICommand RollbackCommand { get; set; }
        IServer Server { get; set; }
        ICommand Expand { get; set; }
        ObservableCollection<IExplorerItemViewModel> Children { get; set; }
        IExplorerTreeItem Parent { get; set; }
        bool CanCreateWorkflowService { get; set; }
        bool AreVersionsVisible { get; set; }
        bool ShowContextMenu { get; set; }
        IShellViewModel ShellViewModel { get;}
        int ChildrenCount { get; }
        Action<IExplorerItemViewModel> SelectAction { get; set; }
        bool? IsFolderChecked { get; set; }
        void AddChild(IExplorerItemViewModel child);
        void RemoveChild(IExplorerItemViewModel child);
        void SelectItem(Guid id, Action<IExplorerItemViewModel> foundAction);
    }
}