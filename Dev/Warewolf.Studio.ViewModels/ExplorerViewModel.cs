
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2015 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

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
	    protected ICollection<IEnvironmentViewModel> _environments;
		private string _searchText;
		private bool _isRefreshing;
		private IExplorerTreeItem _selectedItem;
		private object[] _selectedDataItems;
	    private bool _fromActivityDrop;

	    protected ExplorerViewModelBase()
		{

			RefreshCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(Refresh);
			ClearSearchTextCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(() => SearchText = "");
            CreateFolderCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(CreateFolder);
		}

	    private void CreateFolder()
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
                if (!Equals(_selectedItem, value))
                {
				_selectedItem = value;
          
				OnPropertyChanged(() => SelectedItem);
                    if(SelectedItemChanged!= null)
                    {
                        SelectedItemChanged(this, _selectedItem);
                    }
                }
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

		public virtual ICollection<IEnvironmentViewModel> Environments
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

        public async void RefreshEnvironment(Guid environmentID)
        {
            var environmentViewModel = Environments.FirstOrDefault(model => model.Server.EnvironmentID == environmentID);
            if (environmentViewModel != null)
            {
                IsRefreshing = true;
                if (environmentViewModel.IsConnected)
                {
                    await environmentViewModel.Load();
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        Filter(SearchText);
                    }
                }
                IsRefreshing = false;
            }
        }

	    protected virtual void Refresh()
		{
			IsRefreshing = true;
			Environments.ForEach(async model =>
			{
				if (model.IsConnected)
				{
				    await model.Load();
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
        public event SelectedExplorerItemChanged SelectedItemChanged;



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
	    readonly Action<IExplorerItemViewModel> _selectAction;
	    bool _isLoading;

	    public ExplorerViewModel(IShellViewModel shellViewModel, IEventAggregator aggregator, Action<IExplorerItemViewModel> selectAction = null,bool loadLocalHost=true)
		{
			if (shellViewModel == null)
			{
				throw new ArgumentNullException("shellViewModel");
			}
			var localhostEnvironment = CreateEnvironmentFromServer(shellViewModel.LocalhostServer, shellViewModel);
            _shellViewModel = shellViewModel;
	        _selectAction = selectAction;
            localhostEnvironment.SelectAction = selectAction ?? (a => { });
	        Environments = new ObservableCollection<IEnvironmentViewModel> { localhostEnvironment };
			if(loadLocalHost)
                LoadEnvironment(localhostEnvironment);

			ConnectControlViewModel = new ConnectControlViewModel(shellViewModel.LocalhostServer, aggregator);
			ShowConnectControl = true;
            ConnectControlViewModel.ServerConnected+=ServerConnected;
            ConnectControlViewModel.ServerDisconnected += ServerDisconnected;
			
		}

	    async void ServerConnected(object _, IServer server)
	    {
            IsLoading = true;
            var environmentModel = CreateEnvironmentFromServer(server, _shellViewModel);
            _environments.Add(environmentModel);
	        await environmentModel.Load(IsDeploy);
            OnPropertyChanged(() => Environments);
            AfterLoad(server.EnvironmentID);
            IsLoading = false;
	    }
        public virtual void AfterLoad(Guid environmentID)
        {
            ConnectControlViewModel.IsLoading = false;
        }

	    public bool IsDeploy { get; set; }

	    public virtual bool IsLoading
	    {
	        get
	        {
	            return _isLoading;
	        }
	        set
	        {
	            _isLoading = value;
                OnPropertyChanged(() => IsLoading);
	        }
	    }

	    void ServerDisconnected(object _, IServer server)
        {
            var environmentModel = _environments.FirstOrDefault(model => model.Server.EnvironmentID == server.EnvironmentID);
            if (environmentModel!=null)
            {
                _environments.Remove(environmentModel);
            }
            OnPropertyChanged(()=>Environments);
            AfterLoad(server.EnvironmentID);
        }

		protected virtual async void LoadEnvironment(IEnvironmentViewModel localhostEnvironment,bool isDeploy = false)
		{
			await localhostEnvironment.Connect();
			await localhostEnvironment.Load(isDeploy);
            AfterLoad(localhostEnvironment.Server.EnvironmentID);
		}


		IEnvironmentViewModel CreateEnvironmentFromServer(IServer server, IShellViewModel shellViewModel)
		{
		    if(server != null && server.UpdateRepository != null)
		    {
		        server.UpdateRepository.ItemSaved += Refresh;
		    }
		    return new EnvironmentViewModel(server, shellViewModel,false,_selectAction);
		}
	}
}
