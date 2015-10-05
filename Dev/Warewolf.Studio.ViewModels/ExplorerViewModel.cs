using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Warewolf.Studio.ViewModels
{
	public class ExplorerViewModelBase : BindableBase, IExplorerViewModel
	{
		private ICollection<IEnvironmentViewModel> _environments;
		private string _searchText;
		private bool _isRefreshing;
		private IExplorerTreeItem _selectedItem;
		private object[] _selectedDataItems;
	    bool _fromActivityDrop;

	    protected ExplorerViewModelBase()
		{

			RefreshCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(Refresh);
			ClearSearchTextCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(() => SearchText = "");
            CreateFolderCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(CreateFolder);
		}

	    void CreateFolder()
	    {
	        if(SelectedItem != null)
	        {
	            if(SelectedItem.CreateFolderCommand.CanExecute(null))
	            {
	                SelectedItem.CreateFolderCommand.Execute(null);
	            }
	        }
	    }

        public bool IsFromActivityDrop
        {
            get
            {
                return _fromActivityDrop;
            }
            set
            {
                if (value != _fromActivityDrop)
                {
                    _fromActivityDrop = value;
                    OnPropertyChanged(() => IsFromActivityDrop);
                }
            }
        }
	    public ICommand RefreshCommand { get; set; }

		public bool IsRefreshing
		{
			get
			{
				return _isRefreshing;
			}
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(() => IsRefreshing);
			}
		}

		public bool ShowConnectControl { get; set; }

		public IExplorerTreeItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
          
				OnPropertyChanged(() => SelectedItem);

			}
		}

		public object[] SelectedDataItems
		{
			get { return _selectedDataItems; }
			set
			{
				_selectedDataItems = value;
				OnPropertyChanged(() => SelectedDataItems);
			}
		}

		public ICollection<IEnvironmentViewModel> Environments
		{
			get
			{
				return _environments;
			}
			set
			{
				_environments = value;
				OnPropertyChanged(() => Environments);
			}
		}

		public IEnvironmentViewModel SelectedEnvironment { get; set; }
		public IServer SelectedServer { get { return SelectedEnvironment.Server; } }


		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				if (_searchText == value)
				{
					return;
				}
				_searchText = value;
				Filter(_searchText);
				OnPropertyChanged(() => SearchText);
			}
		}

		public string SearchToolTip
		{
			get
			{
				return Resources.Languages.Core.ExplorerSearchToolTip;
			}
		}

		public string RefreshToolTip
		{
			get
			{
				return Resources.Languages.Core.ExplorerRefreshToolTip;
			}
		}

		protected virtual void Refresh()
		{
			IsRefreshing = true;
			Environments.ForEach(model =>
			{
				if (model.IsConnected)
				{
				    model.Load();
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Filter(SearchText);
                    }
				}
			});
			IsRefreshing = false;
		}

		public void Filter(string filter)
		{
			if (Environments != null)
			{
				foreach (var environmentViewModel in Environments)
				{
					environmentViewModel.Filter(filter);
				}
				OnPropertyChanged(() => Environments);
			}
		}

		public void RemoveItem(IExplorerItemViewModel item)
		{
			if (Environments != null)
			{
				var env = Environments.FirstOrDefault(a => a.Server == item.Server);

				if (env != null)
				{
					if (env.Children.Contains(item))
					{
						env.RemoveChild(item);
					}
					else
						env.RemoveItem(item);
				}
				OnPropertyChanged(() => Environments);
			}
		}

		public event SelectedExplorerEnvironmentChanged SelectedEnvironmentChanged;


		public ICommand ClearSearchTextCommand { get; private set; }
	    public ICommand CreateFolderCommand { get; private set; }

	    public void SelectItem(Guid id)
		{
			foreach (var environmentViewModel in Environments)
			{
				environmentViewModel.SelectItem(id, (a => SelectedItem = a));
                environmentViewModel.SelectAction = a=>SelectedItem =a;
			}
		}
        public void SelectItem(string path)
        {
            foreach (var environmentViewModel in Environments)
            {
                environmentViewModel.SelectItem(path, (a => SelectedItem = a));
                environmentViewModel.SelectAction = a => SelectedItem = a;
            }
        }
		public IList<IExplorerItemViewModel> FindItems(Func<IExplorerItemViewModel, bool> filterFunc)
		{
			return null;
		}
		public IConnectControlViewModel ConnectControlViewModel { get; internal set; }
		protected virtual void OnSelectedEnvironmentChanged(IEnvironmentViewModel e)
		{
			var handler = SelectedEnvironmentChanged;
			if (handler != null) handler(this, e);
		}
	}

	public class ExplorerViewModel : ExplorerViewModelBase
	{
	    readonly IShellViewModel _shellViewModel;

	    public ExplorerViewModel(IShellViewModel shellViewModel, IEventAggregator aggregator)
		{
			if (shellViewModel == null)
			{
				throw new ArgumentNullException("shellViewModel");
			}
			var localhostEnvironment = CreateEnvironmentFromServer(shellViewModel.LocalhostServer, shellViewModel);
            _shellViewModel = shellViewModel;
			Environments = new ObservableCollection<IEnvironmentViewModel> { localhostEnvironment };
			LoadEnvironment(localhostEnvironment);

			ConnectControlViewModel = new ConnectControlViewModel(shellViewModel.LocalhostServer, aggregator);
			ShowConnectControl = true;
            ConnectControlViewModel.ServerConnected+=ServerConnected;
            ConnectControlViewModel.ServerDisconnected += ServerDisconnected;
			
		}

	    async void ServerConnected(object _, IServer server)
	    {
            var environmentModel = CreateEnvironmentFromServer(server, _shellViewModel);
            
            await environmentModel.Load();
            Environments.Add(environmentModel);
	    }

	    void ServerDisconnected(object _, IServer server)
        {
            var environmentModel = Environments.FirstOrDefault(model => model.Server.EnvironmentID == server.EnvironmentID);
            if (environmentModel!=null)
            {
                Environments.Remove(environmentModel);
            }
        }

	    //		void ItemAdded(IExplorerItem obj)
//		{
//
//		}

		private async void LoadEnvironment(IEnvironmentViewModel localhostEnvironment)
		{
			await localhostEnvironment.Connect();
			await localhostEnvironment.Load();
		}


		IEnvironmentViewModel CreateEnvironmentFromServer(IServer server, IShellViewModel shellViewModel)
		{
            return new EnvironmentViewModel(server, shellViewModel);
		}
	}

	public class SingleEnvironmentExplorerViewModel : ExplorerViewModelBase
	{
	    readonly Guid _selectedId;

	    public SingleEnvironmentExplorerViewModel(IEnvironmentViewModel environmentViewModel,Guid selectedId)
		{
	        _selectedId = selectedId;
	        environmentViewModel.SetPropertiesForDialog();
			Environments = new ObservableCollection<IEnvironmentViewModel>
			{
				environmentViewModel
			};
            IsRefreshing = false;
			ShowConnectControl = false;
            SelectItem(_selectedId);
		}

        protected override void Refresh()
        {
            IsRefreshing = true;
            Environments.ForEach(model =>
            {
                if (model.IsConnected)
                {
                    model.LoadDialog(_selectedId);
                    if(!string.IsNullOrEmpty(SearchText))
                    {
                        Filter(SearchText);
                    }
                }
            });
            IsRefreshing = false;
        }
	}
}
