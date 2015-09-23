﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Security;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
	public class EnvironmentViewModel : BindableBase, IEnvironmentViewModel
    {
        ObservableCollection<IExplorerItemViewModel> _children;
        bool _isConnecting;
        bool _isConnected;
        bool _isServerIconVisible;
        bool _isServerUnavailableIconVisible;
        bool _canCreateServerSource;
        private bool _isExpanded;
        private bool _isSelected;
        bool _canCreateFolder;
		bool _canShowServerVersion;
	    private bool _canCreateWorkflowService;
	    readonly IShellViewModel _shellViewModel;

	    public EnvironmentViewModel(IServer server, IShellViewModel shellViewModel)
        {
			if (server == null) throw new ArgumentNullException("server");
            if (shellViewModel == null) throw new ArgumentNullException("shellViewModel");
            _shellViewModel = shellViewModel;
            Server = server;
            //Server.NetworkStateChanged += Server_NetworkStateChanged;
            //server.ItemAddedEvent += ServerItemAddedEvent;
            _children = new ObservableCollection<IExplorerItemViewModel>();
            NewCommand = new DelegateCommand<string>(type => ShellViewModel.NewResource(type,ResourcePath));
            DisplayName = server.ResourceName;
            RefreshCommand = new DelegateCommand(async () => await Load());
            IsServerIconVisible = true;
            SelectAction = a=>{};
            Expand = new DelegateCommand<int?>(clickCount =>
            {
                if (clickCount != null && clickCount == 2)
                {
                    IsExpanded = !IsExpanded;
                }
           
            });
	        server.Connect();
            IsConnected = server.IsConnected;
            //ShowServerVersionCommand = new DelegateCommand(ShowServerVersionAbout);
            CanCreateFolder = Server.UserPermissions == Permissions.Administrator || server.UserPermissions == Permissions.Contribute;
            CreateFolderCommand = new DelegateCommand(CreateFolder);
            Parent = null;
            ResourceType = ResourceType.ServerSource;
            ResourcePath = "";
            ResourceName = DisplayName;
			CanShowServerVersion = true;
	        AreVersionsVisible = false;

        }

	    public IShellViewModel ShellViewModel
	    {
	        get
	        {
	            return _shellViewModel;
	        }
	    }
	    public int ChildrenCount
	    {
	        get
	        {
	            return Children != null ? Children.Count : 0;
	        }
	    }

	    public bool CanCreateWorkflowService
	    {
	        get { return _canCreateWorkflowService; }
	        set
	        {
	            _canCreateWorkflowService = value;
                OnPropertyChanged(() => CanCreateWorkflowService);
	        }
	    }

	    public bool AreVersionsVisible { get; set; }

//	    void ServerItemAddedEvent(IExplorerItem args)
//        {
//			if (args.ResourcePath == args.DisplayName)
//            {
//                Children.Add(new ExplorerItemViewModel(_shellViewModel, Server, new Mock<IExplorerHelpDescriptorBuilder>().Object, null));
//                return;
//            }
//           var found = Find(args.ResourcePath.Substring(0,args.ResourcePath.LastIndexOf("\\", StringComparison.Ordinal)));
//            if(found != null)
//            {
//				if (found.Children.All(a => a.ResourceName != args.DisplayName))
//					_shellViewModel.ExecuteOnDispatcher(() =>
//					found.AddChild(new ExplorerItemViewModel(_shellViewModel, Server, new Mock<IExplorerHelpDescriptorBuilder>().Object, found)
//                    {
//                        ResourceId = args.ResourceId,
//                        ResourceName = args.DisplayName,
//                        Parent = found,
//                        CanCreateDbService = true,
//                        CanCreateDbSource = false,
//                        ResourceType = args.ResourceType
//                    }
//                    ));
//            }
//        }

        IExplorerItemViewModel Find(string resourcePath)
        {
            return Children.Select(explorerItemViewModel => explorerItemViewModel.Find(resourcePath)).FirstOrDefault(found => found != null);
        }

        void CreateFolder()
        {
                IsExpanded = true;
                var id = Guid.NewGuid();
                var name = GetChildNameFromChildren();
                Server.ExplorerRepository.CreateFolder("root", name, id);
                var child = new ExplorerItemViewModel(Server, this, a => { SelectAction(a); }, _shellViewModel)
               {
                   ResourceName = name,
                   ResourceId = id, 
                   ResourceType = ResourceType.Folder,
                   ResourcePath = name
               };
                child.IsSelected = true;
                child.IsRenaming = true;
               _children.Insert(0,child);
               OnPropertyChanged(() => Children);
               
            
        }

        public ICommand ShowServerVersionCommand { get; set; }

	    public Action<IExplorerItemViewModel> SelectAction { get;  set; }

	    public void SelectItem(Guid id, Action<IExplorerItemViewModel> foundAction)
        {
            foreach (var explorerItemViewModel in Children)
            {
                if (explorerItemViewModel.ResourceId == id)
                {
                    if (!explorerItemViewModel.IsExpanded)
                    {
                        explorerItemViewModel.IsExpanded = true;
                    }
                    explorerItemViewModel.IsSelected = true;
                    foundAction(explorerItemViewModel);
                }
                else
                {
                    explorerItemViewModel.SelectItem(id, foundAction);
                }

            }
        }

        public void SelectItem(string path, Action<IExplorerItemViewModel> foundAction)
        {
            foreach (var explorerItemViewModel in Children)
            {
                explorerItemViewModel.Apply(a =>
                {
                    if (a.ResourcePath == path) 
                    {
                        a.IsExpanded = true;
                        foundAction(a);
                    }
                });
            }
        }

        public void SetPropertiesForDialog()
        {
            CanCreateDbService = false;
            CanCreateDbSource = false;
            CanCreateFolder = true;
            CanCreatePluginService = false;
            CanCreatePluginSource = false;
            CanCreateEmailSource = false;
            CanCreateServerSource = false;
            CanCreateWebService = false;
            CanCreateWebSource = false;
            CanDelete = true;
            CanDeploy = false;
            CanRename = true;
            CanRollback = false;
            CanShowVersions = false;
            CanCreateWorkflowService = false;
        }



	    //
//        void Server_NetworkStateChanged(INetworkStateChangedEventArgs args)
//        {
//            switch (args.State)
//            {
//                case ConnectionNetworkState.Connected:
//                {
//                    Server.Connect();
//                    if (!IsConnecting)
//                        ShellViewModel.ExecuteOnDispatcher(async () => await Load());
//                    break;
//                }
//                case ConnectionNetworkState.Disconnected:
//                {
//                    ShellViewModel.ExecuteOnDispatcher(() =>
//                    {
//                        IsConnected = false;
//                        Children = new ObservableCollection<IExplorerItemViewModel>();
//
//                    });
//                }
//                    break;
//                case ConnectionNetworkState.Connecting:
//                {
//                    ShellViewModel.ExecuteOnDispatcher(() =>
//                    {
//                        if (!IsConnecting)
//                            IsConnected = false;
//                        Children = new ObservableCollection<IExplorerItemViewModel>();                        
//                    });
//                }
//                    break;
//                default:
//                {
//                    
//                }
//                    break;
//            }
//        }

        public IServer Server { get; set; }

        public ICommand Expand
        {
            get; set; 
        }

        public ICollection<IExplorerItemViewModel> Children
        {
            get
            {
				if (_children == null) return _children;
				return new ObservableCollection<IExplorerItemViewModel>(_children.Where(a => a.IsVisible));
            }
            set
            {
                _children = new ObservableCollection<IExplorerItemViewModel>( value);
                OnPropertyChanged(() => Children);
                OnPropertyChanged(() => ChildrenCount);
            }
        }

        public IExplorerTreeItem Parent { get; set; }
        public void AddChild(IExplorerItemViewModel child)
        {
            _children.Add(child);
            OnPropertyChanged(() => Children);
        }

        public void RemoveChild(IExplorerItemViewModel child)
        {
            _children.Remove(child);
            OnPropertyChanged(() => Children);
        }

        public ResourceType ResourceType { get; set; }
	    public string ResourcePath { get; set; }

	    public string ResourceName { get; set; }

        public bool IsExpanderVisible
        {
            get
            {
				return Children.Count > 0;
            }
            set
            {
                
            }
        }
        public ICommand NewCommand
        {
            get;
            set;
        }
        public ICommand DeployCommand
        {
            get;
            set;
        }
        public bool CanCreateDbService { get; set; }
        public bool CanCreateDbSource { get; set; }
        public bool CanCreateServerSource
        {
            get
            {
                return _canCreateServerSource;
            }
            set
            {
                _canCreateServerSource = value;
                OnPropertyChanged(()=>CanCreateServerSource);
            }
        }
        public bool CanCreateWebService { get; set; }
        public bool CanCreateWebSource { get; set; }
        public bool CanCreatePluginService { get; set; }
        public bool CanCreatePluginSource { get; set; }
        public bool CanCreateEmailSource { get; set; }
        public bool CanRename { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCreateFolder
        {
            get
            {
				return Server.Permissions != null && Server.Permissions.Any(a => (a.Contribute || a.Administrator) && a.IsServer);
            }
            set
            {
                if (_canCreateFolder != value)
                {
                    _canCreateFolder = value;
                    OnPropertyChanged(() => CanCreateFolder);
                }
            }
        }
        public bool CanDeploy { get; set; }
        public bool CanShowVersions
        {
            get { return false; }
            set
            {
            }
        }
        public bool CanRollback { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                
                _isExpanded = value;
                OnPropertyChanged(() => IsExpanded);
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
		}

	    public bool CanShowServerVersion
	    {
	        get { return _canShowServerVersion; }
	        set
	        {
	            _canShowServerVersion = value;
	            OnPropertyChanged(() => CanShowServerVersion);
	        }
	    }

//	    void ShowServerVersionAbout()
//        {
//            ShellViewModel.ShowAboutBox(Server);
//        }

        string GetChildNameFromChildren()
        {
            const string newFolder = "New Folder";
            int count = 0;
            string folderName = newFolder;
            while (Children.Any(a => a.ResourceName == folderName))
            {
                count++;
                folderName = newFolder + " " + count;
            }
            return folderName;
        }

        public ICommand RenameCommand { get; set; }
        public ICommand CreateFolderCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShowVersionHistory { get; set; }
        public ICommand RollbackCommand { get; set; }
        public string DisplayName
        {
            get;
            set;
        }
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            private set
            {
               
                _isConnected = value;
               
                OnPropertyChanged(() => IsConnected);
            }
        }
        public bool IsLoaded { get; private set; }

        public async Task<bool> Connect()
        {
			if (Server != null)
            {
                IsConnecting = true;
                IsConnected = false;
               // IsConnected = await Server.Connect();
                IsConnected = true;
                IsConnecting = false;
                return IsConnected;
            }
            return false;
        }

        public bool IsConnecting
        {
            get
            {
                return _isConnecting;
            }
            private set
            {
                _isConnecting = value;
               IsServerIconVisible = !value;
               IsServerUnavailableIconVisible = !value;

				OnPropertyChanged(() => IsConnecting);
       
            }
        }

        public async Task<bool> Load()
        {
            var result = await LoadDialog(null);
            return result;
        }

        public async Task<bool> LoadDialog(Guid? selectedId)
        {
            if (IsConnected)
            {
                IsConnecting = true;
                var explorerItems = await Server.LoadExplorer();
				var explorerItemViewModels = CreateExplorerItems(explorerItems.Children, Server, this, selectedId != null);
                Children = explorerItemViewModels;
                
                IsLoaded = true;
                IsConnecting = false;
                IsExpanded = true;
                return IsLoaded;
            }
            return false;
        }



        public void Filter(string filter)
        {
            foreach (var explorerItemViewModel in _children)
            {
                explorerItemViewModel.Filter(filter);
            }

            OnPropertyChanged(() => Children);
        }

        public ICollection<IExplorerItemViewModel> AsList()
        {
            return AsList(Children);
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private ICollection<IExplorerItemViewModel> AsList(ICollection<IExplorerItemViewModel> rootCollection)
        {
            return rootCollection.ToList();

        }
        public void SetItemCheckedState(Guid id, bool state)
        {
			var resource = AsList().FirstOrDefault(a => a.ResourceId == id);
			if (resource != null)
            {
                resource.Checked = state;
            }
        }

        public void RemoveItem(IExplorerItemViewModel vm)
        {
			if (vm.ResourceType != ResourceType.Server)
            {
				var res = AsList(_children).FirstOrDefault(a => a.Children != null && a.Children.Any(b => b.ResourceId == vm.ResourceId));
				if (res != null)
                {
                    res.RemoveChild(res.Children.FirstOrDefault(a => a.ResourceId == vm.ResourceId));
					OnPropertyChanged(() => Children);
                }
            }
        }

        public ICommand RefreshCommand { get; set; }
        public bool IsServerIconVisible
        {
            get
            {
				return _isServerIconVisible && IsConnected;
            }
            set
            {
                _isServerIconVisible = value;
				OnPropertyChanged(() => IsServerIconVisible);
            }
        }
        public bool IsServerUnavailableIconVisible
        {
            get
            {
                return _isServerUnavailableIconVisible && !IsConnected;
            }
            set
            {
                _isServerUnavailableIconVisible = value;
                OnPropertyChanged(() => IsServerIconVisible);
            }
        }

        // ReSharper disable ParameterTypeCanBeEnumerable.Local
        ObservableCollection<IExplorerItemViewModel> CreateExplorerItems(IList<IExplorerItem> explorerItems, IServer server, IExplorerTreeItem parent, bool isDialog = false)
        // ReSharper restore ParameterTypeCanBeEnumerable.Local
        {
            if (explorerItems == null) return new ObservableCollection<IExplorerItemViewModel>();
            var explorerItemModels = new ObservableCollection<IExplorerItemViewModel>();
            if(parent != null)
            {
                parent.Children = new ObservableCollection<IExplorerItemViewModel>();
            }
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var explorerItem in explorerItems)
            {
                var itemCreated = new ExplorerItemViewModel(server, parent, a => { SelectAction(a); }, _shellViewModel)
                {
                    ResourceName = explorerItem.DisplayName,
                    ResourceId = explorerItem.ResourceId,
                    ResourceType = explorerItem.ResourceType,
                    ResourcePath = explorerItem.ResourcePath
                    //Inputs = explorerItem.Inputs,
                    //Outputs = explorerItem.Outputs
                };
                //itemCreated.SetPermissions(server.Permissions);
				if (isDialog)
                {
                    SetPropertiesForDialog(itemCreated);
                }
				itemCreated.Children = CreateExplorerItems(explorerItem.Children, server, itemCreated, isDialog);
                explorerItemModels.Add(itemCreated);
                
            }
			return explorerItemModels;
        }

        private static void SetPropertiesForDialog(ExplorerItemViewModel itemCreated)
        {
            itemCreated.CanCreateDbService = false;
            itemCreated.CanCreateDbSource = false;
            itemCreated.CanCreatePluginService = false;
            itemCreated.CanCreatePluginSource = false;
            itemCreated.CanCreateEmailSource = false;
            itemCreated.CanCreateServerSource = false;
            itemCreated.CanCreateWebService = false;
            itemCreated.CanCreateWebSource = false;
            itemCreated.CanCreateWorkflowService = false;
            itemCreated.CanDeploy = false;
            itemCreated.CanShowVersions = false;
            itemCreated.CanEdit = false;
            itemCreated.CanView = false;
            itemCreated.CanExecute = false;
            itemCreated.CanShowDependencies = false;
        }


        public string RefreshToolTip
        {
            get
            {
                return Resources.Languages.Core.EnvironmentExplorerRefreshToolTip;
            }
        }
//        public IShellViewModel ShellViewModel
//        {
//            get
//            {
//                return _shellViewModel;
//            }
//        }
    }
}