﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dev2.Common.Interfaces;

namespace Warewolf.Studio.ViewModels
{
    public class ExplorerItemNodeViewModel : ExplorerItemViewModel, IExplorerItemNodeViewModel
    {
        bool _textVisibility;
        bool _isMainNode;
        // ReSharper disable TooManyDependencies
        public ExplorerItemNodeViewModel(IServer server, IExplorerItemViewModel parent)
            // ReSharper restore TooManyDependencies
            : base(server, parent,a=>{})
        {
            Self = this;
            Weight = 1;
            IsMainNode = false;
            IsNotMainNode = true;
        }

        #region Implementation of IExplorerItemNodeViewModel

        public IExplorerItemNodeViewModel Self { get; set; }
        public int Weight { get; set; }
        // ReSharper disable ReturnTypeCanBeEnumerable.Global
        public ICollection<ExplorerItemNodeViewModel> NodeChildren
        // ReSharper restore ReturnTypeCanBeEnumerable.Global
        {
            get
            {
                return new ObservableCollection<ExplorerItemNodeViewModel>(Children.Select((a => a as ExplorerItemNodeViewModel)));
            }
        }

        public ICollection<IExplorerItemNodeViewModel> AsList()
        {
            return null;
        }

        public bool IsMainNode
        {
            get
            {
                return _isMainNode;
            }
            set
            {
                _isMainNode = value;
                if (value)
                {
                    IsNotMainNode = false;
                }
            }
        }
        public bool IsNotMainNode { get; set; }

        #endregion

        public bool TextVisibility
        {
            private get
            {
                return _textVisibility;
            }
            set
            {
                _textVisibility = value;
                OnPropertyChanged(() => TextVisibility);
            }
        }
    }
}