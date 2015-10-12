using System;
using System.Collections.ObjectModel;
using Dev2.Common.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace Warewolf.Studio.ViewModels
{
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