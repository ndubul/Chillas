using System;
using System.Collections.Generic;
using System.Windows.Media;
using Dev2;
using Dev2.Common.Interfaces.Help;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Core;

namespace Warewolf.Studio.ViewModels.Help
{
    public class HelpWindowViewModel : BindableBase, IHelpWindowViewModel, IDisposable
    {
        IHelpDescriptorViewModel _currentHelpText;
        readonly IHelpDescriptorViewModel _defaultViewModel;

        public HelpWindowViewModel(IHelpDescriptorViewModel defaultViewModel, IHelpWindowModel model)
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "defaultViewModel", defaultViewModel }, { "model", model } });
            _defaultViewModel = defaultViewModel;
            CurrentHelpText = _defaultViewModel;
            HelpModel = model;
            model.OnHelpTextReceived += OnHelpTextReceived;
        }

        public string HelpText
        {
            get
            {
                return CurrentHelpText.Description;
            }
        }

        public string HelpName
        {
            get
            {
                return CurrentHelpText.Name;
            }
        }

        public DrawingImage HelpImage
        {
            get
            {
                return CurrentHelpText.Icon;
            }
        }

        void OnHelpTextReceived(object sender, IHelpDescriptor desc)
        {
            try
            {
                CurrentHelpText = new HelpDescriptorViewModel(desc);
            }
            catch (Exception)
            {
                CurrentHelpText = _defaultViewModel;
                throw;
            }

        }

        public IHelpWindowModel HelpModel { get; private set; }

        #region Implementation of IHelpWindowViewModel

        /// <summary>
        /// Wpf component binds here
        /// </summary>
        public IHelpDescriptorViewModel CurrentHelpText
        {
            get
            {
                return _currentHelpText;
            }
            set
            {
                _currentHelpText = value;
                OnPropertyChanged(() => HelpName);
                OnPropertyChanged(() => HelpText);
                OnPropertyChanged(() => HelpImage);
            }
        }

        public void UpdateHelpText(string helpText)
        {
            const string StandardStyling = "<html>" +
                                           "<style>" +
                                           "p.MsoNormal, li.MsoNormal, div.MsoNormal {" +
                                           "font-size: 20pt;" +
                                           "font-family: 'Calibri';" +
                                           "margin-bottom: 10pt;" +
                                           "}" +
                                           "</style>";
            const string StandardBodyParagraphOpening = "<body><p class=\"MsoNormal\">";
            const string StandardBodyParagraphClosing = "</p></body></html>";
            var textToDisplay = StandardStyling + StandardBodyParagraphOpening + helpText + StandardBodyParagraphClosing;
            CurrentHelpText = new HelpDescriptorViewModel(new HelpDescriptor("", textToDisplay, null));
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // ReSharper disable once UnusedParameter.Local
        void Dispose(bool disposing)
        {
            HelpModel.OnHelpTextReceived -= OnHelpTextReceived;
        }

        #endregion
    }
}
