using System;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.DB;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public abstract class SourceBaseImpl<T> : BindableBase, ISourceBase<T>, IDockViewModel
    {
        string _header;

        protected SourceBaseImpl(ResourceType? image)
        {
            Image = image;
        }

        #region Implementation of ISourceBase<T>

        public T Item { get; set; }
        public bool HasChanged { get { return Item != null && !Item.Equals(ToModel()); } }

        abstract  public T ToModel();

        #endregion

        #region Implementation of IDockAware

        public string Header
        {
            get
            {
                if (HasChanged)
                {
                    return _header + " *";
                }
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged(() => Header);
            }
        }
        public ResourceType? Image { get; private set; }

        #endregion

        public bool IsActive { get; set; }
        public abstract bool CanSave();

        public event EventHandler IsActiveChanged;
        public abstract void UpdateHelpDescriptor(string helpText);
    }
}