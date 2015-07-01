using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Warewolf.Studio.ViewModels
{
    public static class ViewModelUtils
    {
        
        public static void RaiseCanExecuteChanged(ICommand commandForCanExecuteChange)
        {
            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RaiseCanExecuteChangedInternal(commandForCanExecuteChange);
                });
            }
            else
            {
                RaiseCanExecuteChangedInternal(commandForCanExecuteChange);
            }
        }

        static void RaiseCanExecuteChangedInternal(ICommand commandForCanExecuteChange)
        {
            var command = commandForCanExecuteChange as DelegateCommand;
            if(command != null)
            {
                command.RaiseCanExecuteChanged();
            }
        }
    }
}