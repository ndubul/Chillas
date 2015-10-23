﻿
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
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Dev2;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Warewolf.Core;

namespace Warewolf.Studio.ViewModels
{
    [ComVisible(false)]
    public class ManageDatabaseServiceViewModel : SourceBaseImpl<IDatabaseService>, IManageDbServiceViewModel
    {
        readonly IDbServiceModel _model;
        readonly IRequestServiceNameViewModel _saveDialog;
        ICollection<IDbSource> _sources;
        IDbSource _selectedSource;
        IDbAction _selectedAction;
        IDbAction _refreshSelectedAction;
        ICollection<IServiceInput> _inputs;
        private bool _inputsRequired;
        DataTable _testResults;
        private bool _isTesting;
        IList<IServiceOutputMapping> _outputMapping;
        bool _canSelectProcedure;
        bool _canEditMappings;
        bool _canTest;
        ICollection<IDbAction> _avalaibleActions;
        bool _testSuccessful;
        bool _testResultsAvailable;
        string _errorText;
        string _recordsetName;
        private bool _isRefreshing;
        private bool _isTestResultsEmptyRows;
        private bool _isInputsEmptyRows;
        private bool _isOutputMappingEmptyRows;
        private bool _showRecordSet;
        string _headerText;
        string _resourceName;
        IDatabaseService _dbService;
        string _name;
        string _path;

        /// <exception cref="ArgumentNullException"><paramref name="model"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="saveDialog"/> is <see langword="null" />.</exception>
        public ManageDatabaseServiceViewModel(IDbServiceModel model, IRequestServiceNameViewModel saveDialog)
            : base(ResourceType.DbService)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            _model = model;
            _saveDialog = saveDialog;
            CanEditSource = false;
            IsNew = true;
            CreateNewSourceCommand = new DelegateCommand(model.CreateNewSource);
            EditSourceCommand = new DelegateCommand(() => model.EditSource(SelectedSource));

            Header = Resources.Languages.Core.DatabaseServiceDBSourceTabHeader;
            HeaderText = Resources.Languages.Core.DatabaseServiceDBSourceTabHeader;
            ResourceName = HeaderText;
            TestProcedureCommand = new DelegateCommand(TestAction, CanTestProcedure);
            Inputs = new ObservableCollection<IServiceInput>();
            SaveCommand = new DelegateCommand(Save, CanSave);
            RefreshCommand = new DelegateCommand(Refresh);
            IsRefreshing = false;
            IsTesting = false;
            IsTestResultsEmptyRows = false;
            IsInputsEmptyRows = false;
            IsOutputMappingEmptyRows = false;
            ShowRecordSet = true;
            try
            {
                Sources = _model.RetrieveSources();
            }
            catch (Exception ex)
            {
                Exception exception = new Exception();
                if (ex.InnerException != null)
                {
                    exception = ex.InnerException;
                }
                ErrorText = exception.Message;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="model" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="saveDialog"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="service"/> is <see langword="null" />.</exception>
        public ManageDatabaseServiceViewModel(IDbServiceModel model, IRequestServiceNameViewModel saveDialog, IDatabaseService service)
            : this(model, saveDialog)
        {
            if (saveDialog == null&& model==null)
            {
                throw new ArgumentNullException("saveDialog");
            }
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            IsNew = false;
            _dbService = service;
            FromService(service);
            PerformRefresh();
            OnPropertyChanged(() => Header);
        }

        public ICommand RefreshCommand { get; set; }

        private void Refresh()
        {
            IsRefreshing = true;
            PerformRefresh();
            IsRefreshing = false;
        }

        void TestAction()
        {
            try
            {
                IsTesting = true;
                TestResults = _model.TestService(ToModel());
                if (TestResults != null)
                {
                    CanEditMappings = true;
                    TestResultsAvailable = true;
                    OutputMapping =
                        new ObservableCollection<IServiceOutputMapping>(GetDbOutputMappingsFromTable(TestResults));
                    TestSuccessful = true;
                }
                ErrorText = "";
                IsTesting = false;
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                OutputMapping = null;
                TestResultsAvailable = false;
                CanEditMappings = false;
                Inputs = null;
                TestSuccessful = false;
                IsTesting = false;
            }
        }



        void FromService(IDatabaseService service)
        {
            Item = service;
            Id = service.Id;
            Name = service.Name;
            Path = service.Path;
            SelectedSource = Sources.FirstOrDefault(a=>a.Id == service.Source.Id);
            service.Source = SelectedSource;
            SelectedAction = service.Action;
            Inputs = service.Inputs;
            OutputMapping = service.OutputMappings;
            Header = Name;
            CanEditMappings = true;
        }

        public bool IsTesting
        {
            get { return _isTesting; }
            set
            {
                _isTesting = value;
                OnPropertyChanged(() => IsTesting);
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(() => IsRefreshing);
            }
        }

        public bool ShowRecordSet
        {
            get { return _showRecordSet; }
            set
            {
                _showRecordSet = value;
                OnPropertyChanged(() => ShowRecordSet);
            }
        }

        public bool IsTestResultsEmptyRows
        {
            get { return _isTestResultsEmptyRows; }
            set
            {
                _isTestResultsEmptyRows = value;
                OnPropertyChanged(() => IsTestResultsEmptyRows);
            }
        }

        public bool IsInputsEmptyRows
        {
            get { return _isInputsEmptyRows; }
            set
            {
                _isInputsEmptyRows = value;
                OnPropertyChanged(() => IsInputsEmptyRows);
            }
        }

        public bool IsOutputMappingEmptyRows
        {
            get { return _isOutputMappingEmptyRows; }
            set
            {
                _isOutputMappingEmptyRows = value;
                OnPropertyChanged(() => IsOutputMappingEmptyRows);
            }
        }

        bool CanTestProcedure()
        {
            return SelectedAction != null;
        }

        public override bool CanSave()
        {
            return TestSuccessful;
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

        public bool InputsRequired
        {
            get { return _inputsRequired; }
            set
            {
                _inputsRequired = value;
                OnPropertyChanged(() => InputsRequired);
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

        public string RecordsetName
        {
            get
            {
                return _recordsetName;
            }
            set
            {
                _recordsetName = value;
                if (OutputMapping != null && OutputMapping.Count > 0)
                {
                    foreach (var dbOutputMapping in OutputMapping)
                    {
                        dbOutputMapping.RecordSetName = _recordsetName;
                    }
                }
                OnPropertyChanged(() => RecordsetName);
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

        List<IServiceOutputMapping> GetDbOutputMappingsFromTable(DataTable testResults)
        {
            List<IServiceOutputMapping> mappings = new List<IServiceOutputMapping>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            for(int i = 0; i < testResults.Columns.Count; i++)
            {
                var column = testResults.Columns[i];
                if(i == 0)
                {
                    RecordsetName = SelectedAction.Name;
                }
                var dbOutputMapping = new ServiceOutputMapping(column.ToString(), column.ToString()) { RecordSetName = RecordsetName };
                mappings.Add(dbOutputMapping);
            }

            return mappings;
        }

        public override void Save()
        {
            try
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
                        IsNew = false;
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
            catch (Exception err)
            {
                ErrorText = err.Message;
            }
        }

        public bool IsNew { get; set; }

        public Guid Id { get; set; }

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

        #region Implementation of IManageDbServiceViewModel

        public ICollection<IDbSource> Sources
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
        public IDbSource SelectedSource
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
                    CanSelectProcedure = value != null;
                    PerformRefresh();
                    SelectedAction = null;
                    CanEditSource = true;
                    OnPropertyChanged(() => SelectedSource);
                    OnPropertyChanged(() => CanEditSource);
                    ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
                }

            }
        }

        private void PerformRefresh()
        {
            try
            {
                _refreshSelectedAction = SelectedAction;
                AvalaibleActions = _model.GetActions(SelectedSource);
                SelectedAction = AvalaibleActions.FirstOrDefault(action => _refreshSelectedAction != null && action.Name == _refreshSelectedAction.Name);
                ErrorText = "";
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
                AvalaibleActions = null;
            }
        }

        public IDbAction SelectedAction
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
                ViewModelUtils.RaiseCanExecuteChanged(TestProcedureCommand);
                OnPropertyChanged(() => SelectedAction);
            }
        }
        public ICollection<IDbAction> AvalaibleActions
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

        public string DataSourceHeader
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceDBSourceHeader;

            }
        }
        public string DataSourceActionHeader
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceActionHeader;
            }
        }
        public ICommand EditSourceCommand { get; private set; }
        public bool CanEditSource { get; private set; }
        public string NewButtonLabel
        {
            get
            {
                return Resources.Languages.Core.New;
            }
        }

        public string MappingsHeader
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceMappingsHeader;
            }
        }
        public string TestHeader
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceTestHeader;
            }
        }
        public string InputsLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceInputsHeader;
            }
        }

        public string OutputsLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseServiceOutputsLabel;
            }
        }

        public ICollection<IServiceInput> Inputs
        {
            get
            {
                return _inputs;
            }
            private set
            {
                _inputs = value;
                IsInputsEmptyRows = true;
                InputsRequired = false;
                if (_inputs != null && _inputs.Count >= 1)
                {
                    InputsRequired = true;
                    IsInputsEmptyRows = false;
                }
                OnPropertyChanged(() => Inputs);
            }
        }
        public DataTable TestResults
        {
            get
            {
                return _testResults;
            }
            set
            {
                _testResults = value;
                IsTestResultsEmptyRows = true;
                if (_testResults != null && _testResults.Rows.Count >= 1)
                {
                    IsTestResultsEmptyRows = false;
                }
                OnPropertyChanged(() => TestResults);
            }
        }
        public ICommand CreateNewSourceCommand { get; set; }
        public ICommand TestProcedureCommand { get; set; }
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
                ShowRecordSet = false;
                TestResultsAvailable = false;
                if (_outputMapping != null)
                {
                    if (_outputMapping.Count >= 1)
                    {
                        TestResultsAvailable = true;
                        IsOutputMappingEmptyRows = false;
                        ShowRecordSet = true;
                    }
                }

                OnPropertyChanged(() => OutputMapping);
                ViewModelUtils.RaiseCanExecuteChanged(SaveCommand);
            }
        }
        public ICommand SaveCommand { get; set; }
        public bool CanSelectProcedure
        {
            get
            {
                return _canSelectProcedure;
            }
            set
            {
                _canSelectProcedure = value;
                OnPropertyChanged(() => CanSelectProcedure);
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

        #endregion

        #region Implementation of ISourceBase<IDatabaseService>

       
        public override IDatabaseService ToModel()
        {
            if (Item == null || Item.Id.Equals(Guid.Empty))
            {
                Item = ToService();
                return Item;
            }
            return new DatabaseService
            {
                Action = SelectedAction,
                Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList(),
                OutputMappings = OutputMapping,
                Source = SelectedSource,
                Name = Name,
                Path = Path,
                Id = _dbService == null ? Guid.NewGuid() : _dbService.Id
            };
            
        }

        IDatabaseService ToService()
        {
            if (_dbService == null)
                return new DatabaseService
                {
                    Action = SelectedAction,
                    Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList(),
                    OutputMappings = OutputMapping,
                    Source = SelectedSource,
                    Name = Name,
                    Path = Path,
                    Id = _dbService == null ? Guid.NewGuid() : _dbService.Id
                }
            ;
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                _dbService.Action = SelectedAction;
                _dbService.Inputs = Inputs == null ? new List<IServiceInput>() : Inputs.ToList();
                _dbService.OutputMappings = OutputMapping;
                _dbService.Source = SelectedSource;
                _dbService.Name = Name;
                _dbService.Path = Path;
                _dbService.Id = _dbService == null ? Guid.NewGuid() : _dbService.Id;
                return _dbService;
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


    }
}
