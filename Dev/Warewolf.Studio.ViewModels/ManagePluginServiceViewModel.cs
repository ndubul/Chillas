
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Communication;
using Dev2.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.Practices.Prism.Commands;
using Warewolf.Core;

namespace Warewolf.Studio.ViewModels
{
    public class ManagePluginServiceViewModel : SourceBaseImpl<IPluginService>, IPluginServiceViewModel
    {
        readonly IPluginServiceModel _model;
        readonly IRequestServiceNameViewModel _saveDialog;
        string _mappingsHeader;
        ICollection<IServiceInput> _inputs;
        IList<IServiceOutputMapping> _outputMapping;
        string _recordsetName;
        bool _isInputsEmptyRows;
        bool _isOutputMappingEmptyRows;
        ICollection<IPluginSource> _sources;
        IPluginSource _selectedSource;
        IPluginAction _selectedAction;
        IPluginAction _refreshSelectedAction;
        IPluginService _pluginService;
        ICollection<IPluginAction> _avalaibleActions;
#pragma warning disable 649
        string _pluginSourceHeader;
        string _pluginSourceActionHeader;
        bool _canEditSource;
        string _newButtonLabel;
        string _testHeader;
        string _inputsLabel;
        string _outputsLabel;
#pragma warning restore 649
        bool _isRefreshing;
        bool _inputsRequired;
        string _testResults;
        bool _isTestResultsEmptyRows;
        bool _isTesting;
        bool _canSelectMethod;
        bool _canEditMappings;
        bool _canTest;
        string _path;
        string _name;
        Guid _id;
        bool _testSuccessful;
        bool _testResultsAvailable;
        string _errorText;
        bool _showResults;
        RecordsetList _responseService;
        bool _canSave;
        ICollection<INamespaceItem> _nameSpaces;
        INamespaceItem _selectedNamespace;
        ICollection<NameValue> _inputValues;
        string _headerText;
        string _resourceName;
        string _testResultString;

        #region Implementation of IServiceMappings

        public ManagePluginServiceViewModel(IPluginServiceModel model, IRequestServiceNameViewModel saveDialog)
            : this(model)
        {

            _saveDialog = saveDialog;


        }

        public ManagePluginServiceViewModel(IPluginServiceModel model)
            : base(ResourceType.PluginService)
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "model", model } });
            _model = model;
            IsNew = true;
            Id = Guid.NewGuid();
            Inputs = new ObservableCollection<IServiceInput>();
            OutputMapping = new ObservableCollection<IServiceOutputMapping>();
            AvalaibleActions = new ObservableCollection<IPluginAction>();
            NameSpaces = new ObservableCollection<INamespaceItem>();
            Sources = _model.RetrieveSources();
            Header = Resources.Languages.Core.PluginServiceNewHeaderLabel;
            HeaderText = Resources.Languages.Core.PluginServiceNewHeaderLabel;
            ResourceName = HeaderText;
            RefreshCommand = new DelegateCommand(Refresh);
            ErrorText = "";
            TestPluginCommand = new DelegateCommand(() => Test(_model));
            SaveCommand = new DelegateCommand(() => Save(), CanSave);
            CreateNewSourceCommand = new DelegateCommand(() => _model.CreateNewSource());
            EditSourceCommand = new DelegateCommand(() => _model.EditSource(SelectedSource));
        }


        public ManagePluginServiceViewModel(IPluginServiceModel model, IPluginService selectedSource)
            : this(model)
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "model", model } });
            _model = model;
            _pluginService = selectedSource;
            Item = selectedSource;
            IsNew = false;
            FromModel(selectedSource);
        }

        void FromModel(IPluginService selectedConnector)
        {
            SelectedSource = Sources.FirstOrDefault(a => a.Id == selectedConnector.Source.Id);
            Id = selectedConnector.Id;
            Name = selectedConnector.Name;
            Path = selectedConnector.Path;
            Inputs = selectedConnector.Inputs;
            OutputMapping = selectedConnector.OutputMappings;
           
            SelectedNamespace = NameSpaces.FirstOrDefault(a=>a.FullName== selectedConnector.Action.FullName);
            SelectedAction = AvalaibleActions.FirstOrDefault(a => a.Method == selectedConnector.Action.Method);
            Item.Source = SelectedSource;
            Item.Action = SelectedAction;
            Header = selectedConnector.Name;
        }

        void Save()
        {
            if (IsNew)
            {
                var saveOutPut = _saveDialog.ShowSaveDialog();
                if (saveOutPut == MessageBoxResult.OK || saveOutPut == MessageBoxResult.Yes)
                {
                    Name = _saveDialog.ResourceName.Name;
                    Path = _saveDialog.ResourceName.Path;
                    Id = Guid.NewGuid();
                    _model.SaveService(ToModel());
                    Item = ToModel();
                    Header = Path + Name;

                }
            }
            else
            {
                _model.SaveService(ToModel());
                Item = ToModel();
            }
            ErrorText = "";
        }

        public bool IsNew { get; set; }

        public string MappingsHeader
        {
            get
            {
                return _mappingsHeader;
            }
        }
        public ICollection<IServiceInput> Inputs
        {
            get
            {
                return _inputs;
            }
            set
            {
                _inputs = value;
                IsInputsEmptyRows = true;
                if (_inputs != null && _inputs.Count >= 1)
                {
                    IsInputsEmptyRows = false;
                }
                OnPropertyChanged(() => Inputs);
            }
        }
        public IList<IServiceOutputMapping> OutputMapping
        {
            get
            {
                return _outputMapping;
            }
            set
            {
                _outputMapping = value;
                IsOutputMappingEmptyRows = true;
                if (_outputMapping != null)
                {
                    if (_outputMapping.Count >= 1)
                    {
                        IsOutputMappingEmptyRows = false;
                    }
                }
                OnPropertyChanged(() => OutputMapping);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
        public string RecordsetName
        {
            get
            {
                return _recordsetName;
            }
            set
            {
                _recordsetName = value;
                OnPropertyChanged(() => RecordsetName);
            }
        }
        public bool IsInputsEmptyRows
        {
            get
            {
                return _isInputsEmptyRows;
            }
            set
            {
                _isInputsEmptyRows = value;
                OnPropertyChanged(() => IsInputsEmptyRows);
            }
        }
        public bool IsOutputMappingEmptyRows
        {
            get
            {
                return _isOutputMappingEmptyRows;
            }
            set
            {
                _isOutputMappingEmptyRows = value;
                OnPropertyChanged(() => IsOutputMappingEmptyRows);
            }
        }

        #endregion

        #region Implementation of IPluginServiceViewModel

        public ICollection<IPluginSource> Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                OnPropertyChanged(() => Sources);
            }
        }
        public IPluginSource SelectedSource
        {
            get
            {
                return _selectedSource;
            }
            set
            {
                if (!Equals(_selectedSource, value))
                {
                    _selectedSource = value;
                    CanTest = false;
                    CanEditMappings = false;
                    CanSelectMethod = false;
                    CanEditNamespace = true;
                    NameSpaces = new ObservableCollection<INamespaceItem>(_model.GetNameSpaces(value)).OrderBy(a => a.FullName).ToList();
                    OnPropertyChanged(() => SelectedSource);
                    ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
                }
            }
        }

        void Test(IPluginServiceModel model)
        {
            try
            {
                IsTesting = true;
                var serializer = new Dev2JsonSerializer();
                var testService = model.TestService(ToModel());
                ResponseService = serializer.Deserialize<RecordsetList>(testService);
                if (ResponseService.Any(recordset => recordset.HasErrors))
                {
                    var errorMessage = string.Join(Environment.NewLine, ResponseService.Select(recordset => recordset.ErrorMessage));
                    throw new Exception(errorMessage);
                }
                TestResultString = testService;
                UpdateMappingsFromResponse();
                ErrorText = "";
                CanEditMappings = true;
                _canSave = true;
                var delegateCommand = SaveCommand as DelegateCommand;
                if(delegateCommand != null)
                {
                    delegateCommand.RaiseCanExecuteChanged();
                }
                IsTesting = false;

            }
            catch (Exception err)
            {
                ErrorText = err.Message;
                OutputMapping = new ObservableCollection<IServiceOutputMapping>();
                IsTesting = false;
                CanEditMappings = false;
            }


        }

        public string TestResultString
        {
            get
            {
                return _testResultString;
            }
            set
            {
                SetProperty(ref _testResultString, value);
            }
        }

        void UpdateMappingsFromResponse()
        {
            if (ResponseService != null)
            {
                Response = ResponseService;
                // ReSharper disable MaximumChainedReferences
                var outputMapping = ResponseService.SelectMany(recordset => recordset.Fields, (recordset, recordsetField) =>
                {
                    RecordsetName = recordset.Name;
                    var serviceOutputMapping = new ServiceOutputMapping(recordsetField.Name, recordsetField.Alias) { RecordSetName = recordset.Name };
                    return serviceOutputMapping;
                }).Cast<IServiceOutputMapping>().ToList();
                // ReSharper restore MaximumChainedReferences

                OutputMapping = outputMapping;

            }
        }
        public RecordsetList ResponseService
        {
            get
            {
                return _responseService;
            }
            set
            {
                _responseService = value;
                OnPropertyChanged(() => ResponseService);
            }
        }

        public IPluginAction SelectedAction
        {
            get
            {
                return _selectedAction;
            }
            set
            {
                if ((!Equals(value, _selectedAction) && _selectedAction != null) || _selectedAction == null)
                {
                    IsTestResultsEmptyRows = false;
                    TestResultsAvailable = false;
                    CanEditMappings = false;
                    TestSuccessful = false;
                    OutputMapping = null;
                }
                _selectedAction = value;
                CanTest = _selectedAction != null;
                Inputs = _selectedAction != null ? _selectedAction.Inputs : new Collection<IServiceInput>();
                if (_selectedAction != null)
                {
                    InputValues = new ObservableCollection<NameValue>(_selectedAction.Variables.Select(a => a as NameValue));
                }

                OnPropertyChanged(() => SelectedAction);
            }
        }
        public ICollection<NameValue> InputValues
        {
            get
            {
                return _inputValues;
            }
            set
            {
                _inputValues = value;
                OnPropertyChanged(() => InputValues);
            }
        }
        public ICollection<IPluginAction> AvalaibleActions
        {
            get
            {
                return _avalaibleActions;
            }
            set
            {
                _avalaibleActions = value;
                OnPropertyChanged(() => AvalaibleActions);
            }
        }
        public string PluginSourceHeader
        {
            get
            {
                return _pluginSourceHeader;
            }
        }
        public string PluginSourceActionHeader
        {
            get
            {
                return _pluginSourceActionHeader;
            }
        }
        public ICommand EditSourceCommand
        {
            get;
            set;
        }
        public bool CanEditSource
        {
            get
            {
                return _canEditSource;
            }
            set
            {
                _canEditSource = value;
                OnPropertyChanged(() => CanEditSource);
            }
        }
        public bool CanEditNamespace
        {
            get
            {
                return _canEditSource;
            }
            set
            {
                _canEditSource = value;
                OnPropertyChanged(() => CanEditNamespace);
            }
        }
        public string NewButtonLabel
        {
            get
            {
                return _newButtonLabel;
            }
        }
        public string TestHeader
        {
            get
            {
                return _testHeader;
            }
        }
        public string InputsLabel
        {
            get
            {
                return _inputsLabel;
            }
        }
        public string OutputsLabel
        {
            get
            {
                return _outputsLabel;
            }
        }
        public ICommand RefreshCommand { get; set; }
        private void Refresh()
        {
            IsRefreshing = true;
            PerformRefresh();
            IsRefreshing = false;
        }
        private void PerformRefresh()
        {
            try
            {
                _refreshSelectedAction = SelectedAction;
                AvalaibleActions = new ObservableCollection<IPluginAction>(_model.GetActions(SelectedSource, SelectedNamespace));
                SelectedAction = AvalaibleActions.FirstOrDefault(action => _refreshSelectedAction != null && action.FullName == _refreshSelectedAction.FullName);
                ErrorText = "";
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                AvalaibleActions = null;
            }
        }
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
        public bool InputsRequired
        {
            get
            {
                return _inputsRequired;
            }
            set
            {
                _inputsRequired = value;
                OnPropertyChanged(() => InputsRequired);
            }
        }
        public string TestResults
        {
            get
            {
                return _testResults;
            }
            set
            {
                _testResults = value;
                OnPropertyChanged(() => TestResults);
            }
        }
        public bool IsTestResultsEmptyRows
        {
            get
            {
                return _isTestResultsEmptyRows;
            }
            set
            {
                _isTestResultsEmptyRows = value;
                OnPropertyChanged(() => IsTestResultsEmptyRows);

            }
        }
        public ICommand CreateNewSourceCommand { get; set; }
        public ICommand TestPluginCommand { get; set; }
        public bool IsTesting
        {
            get
            {
                return _isTesting;
            }
            set
            {
                _isTesting = value;
                OnPropertyChanged(() => IsTesting);
            }
        }
        public ICommand SaveCommand { get; set; }
        public bool CanSelectMethod
        {
            get
            {
                return _canSelectMethod;
            }
            set
            {
                _canSelectMethod = value;
                OnPropertyChanged(() => CanSelectMethod);
            }
        }
        public bool CanEditMappings
        {
            get
            {
                return _canEditMappings;
            }
            set
            {
                _canEditMappings = value;
                OnPropertyChanged(() => CanEditMappings);
            }
        }
        public bool CanTest
        {
            get
            {
                return _canTest;
            }
            set
            {
                _canTest = value;
                OnPropertyChanged(() => CanTest);
            }
        }
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged(() => Path);
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }
        public Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }
        public bool TestSuccessful
        {
            get
            {
                return _testSuccessful;
            }
            set
            {
                _testSuccessful = value;
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
                OnPropertyChanged(() => TestSuccessful);
            }
        }
        public bool TestResultsAvailable
        {
            get
            {
                return _testResultsAvailable;
            }
            set
            {
                _testResultsAvailable = value;
                OnPropertyChanged(() => TestResultsAvailable);
            }
        }
        public string ErrorText
        {
            get
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                OnPropertyChanged(() => ErrorText);
            }
        }
        public bool ShowResults
        {
            get
            {
                return _showResults;
            }
            set
            {
                _showResults = value;
                OnPropertyChanged(() => ShowResults);
            }
        }
        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
            }
        }
        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                OnPropertyChanged(_resourceName);
            }
        }

        #endregion

        #region Overrides of SourceBaseImpl<IPluginService>

        public override IPluginService ToModel()
        {
            if (Item == null || Item.Id.Equals(Guid.Empty))
            {
                Item = ToService();
                return Item;
            }
            return new PluginServiceDefinition
            {
                Action = SelectedAction,
                Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList(),
                OutputMappings = OutputMapping == null ? new List<IServiceOutputMapping>() : OutputMapping.ToList(),
                Source = SelectedSource,
                Name = Name,
                Path = Path,
                Id = Item == null ? Guid.NewGuid() : Item.Id
            };

        }

        IPluginService ToService()
        {
            if (_pluginService == null)
                return new PluginServiceDefinition()
                {
                    Name = Name,
                    Action = SelectedAction,
                    Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList(),
                    OutputMappings = OutputMapping == null ? new List<IServiceOutputMapping>() : OutputMapping.ToList(),
                    Source = SelectedSource,
                    Path = Path,
                    Id = _pluginService == null ? Guid.NewGuid() : _pluginService.Id
                };
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _pluginService.Name = Name;
                _pluginService.Action = SelectedAction;
                _pluginService.Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList();
                _pluginService.OutputMappings = OutputMapping == null ? new List<IServiceOutputMapping>() : OutputMapping.ToList();
                _pluginService.Source = SelectedSource;
                _pluginService.Path = Path;
                _pluginService.Id = _pluginService == null ? Guid.NewGuid() : _pluginService.Id;
                return _pluginService;
            }
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IMainViewModel>();
            if (mainViewModel != null)
            {
                mainViewModel.HelpViewModel.UpdateHelpText(helpText);
            }
        }

        #endregion

        public RecordsetList Response { get; set; }
        public override bool CanSave()
        {
            return _canSave;
        }
        public ICollection<INamespaceItem> NameSpaces
        {
            get
            {
                return _nameSpaces;
            }
            set
            {
                _nameSpaces = value;
                OnPropertyChanged(() => NameSpaces);
            }
        }
        public INamespaceItem SelectedNamespace
        {
            get
            {
                return _selectedNamespace;
            }
            set
            {
                _selectedNamespace = value;
                if (SelectedNamespace != null)
                {
                    CanTest = false;
                    CanEditMappings = false;
                    CanSelectMethod = true;
                    AvalaibleActions = new ObservableCollection<IPluginAction>(_model.GetActions(SelectedSource, value));
                }
                OnPropertyChanged(() => SelectedNamespace);
            }
        }
    }
}
