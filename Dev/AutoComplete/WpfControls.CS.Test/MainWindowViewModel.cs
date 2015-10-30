

using System.Collections.ObjectModel;

namespace WpfControls.CS.Test
{
    using System;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows;

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region "Fields"

        private ICommand _cancelCommand;
        private string _fileName;
        private ICommand _openCommand;

        private string _selectedPath;
        #endregion

        #region "Constructor"
        public MainWindowViewModel()
        {
            SelectedPath = "";
            Dev2Provider = new Dev2TrieSugggestionProvider
            {
                VariableList = new ObservableCollection<string>
                {
                    "[[a]]",
                    "[[b]]",
                    "[[Rec().a]]",
                    "[[Rec().b]]",
                    "[[Rec()]]",
                    "[[Bob()]]",
                    "[[The()]]",
                    "[[Builder()]]",
                    "[[Can()]]",
                    "[[We()]]",
                    "[[Build()]]",
                    "[[Build().it]]",

                }
            };
        }
        #endregion

        #region "Events"

        public Dev2TrieSugggestionProvider Dev2Provider { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region "Properties"

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(ExecuteCancelCommand, null);
                }
                return _cancelCommand;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new DelegateCommand(ExecuteOpenCommand, null);
                }
                return _openCommand;
            }
        }

        public string SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; RaisePropertyChanged("SelectedPath"); }
        }
        #endregion

        #region "Methods"

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ExecuteCancelCommand(object param)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteOpenCommand(object param)
        {
            try
            {
                Process.Start(SelectedPath);
                Application.Current.Shutdown();
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}
