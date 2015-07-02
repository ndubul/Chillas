
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
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2.Activities.Designers2.Core;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Common.Interfaces.Infrastructure.Providers.Validation;
using Dev2.Data.SystemTemplates.Models;
using Dev2.DataList;
using Dev2.DataList.Contract;
using Dev2.Providers.Validation.Rules;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.Core.Activities.Utils;
using Dev2.TO;
using Dev2.Validation;

namespace Dev2.Activities.Designers2.Decision
{
    public class DecisionDesignerViewModel : ActivityCollectionDesignerViewModel<DecisionTO>
    {
        public Func<string> GetDatalistString = () => DataListSingleton.ActiveDataList.Resource.DataList;
        readonly IList<string> _requiresSearchCriteria = new List<string> { "Doesn't Contain", "Contains", "=", "<> (Not Equal)", "Ends With", "Doesn't Start With", "Doesn't End With", "Starts With", "Is Regex", "Not Regex", ">", "<", "<=", ">=" };

        public DecisionDesignerViewModel(ModelItem modelItem)
            : base(modelItem)
        {
            AddTitleBarLargeToggle();
            AddTitleBarHelpToggle();

            WhereOptions = new ObservableCollection<string>(FindRecsetOptions.FindAll().Select(c => c.HandlesType()));
            SearchTypeUpdatedCommand = new DelegateCommand(OnSearchTypeChanged);
            Tos = new  ObservableCollection<DecisionTO>(new[]{new DecisionTO() });
            Tos.CollectionChanged += Tos_CollectionChanged;

            
  
        }

        void Tos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            var stack = SetupTos(_observables);
            ExpressionText = compiler.ConvertModelToJson(stack).ToString();
        }

        public override string CollectionName { get { return "ResultsCollection"; } }

        public ICommand SearchTypeUpdatedCommand { get; private set; }

        public ObservableCollection<string> WhereOptions { get; private set; }

        string DisplayText { get { return GetProperty<string>(); } }
        string TrueArmText { get { return GetProperty<string>(); } }
        string FalseArmText { get { return GetProperty<string>(); } }
        string ExpressionText
        {
            get { return GetProperty<string>(); } 
            set {SetProperty(value);}

        }
        public ObservableCollection<DecisionTO> Tos
        {

            get
            {
                return _observables;
            }
            set
            {
                IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
                _observables = value;
                var stack = SetupTos(value);
                ExpressionText = compiler.ConvertModelToJson(stack).ToString();
            }
        }

        ObservableCollection<DecisionTO> ToObservableCollection()
        {
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
            var val = new StringBuilder(Dev2DecisionStack.ExtractModelFromWorkflowPersistedData(ExpressionText));
            var decisions = compiler.ConvertFromJsonToModel<Dev2DecisionStack>(val);
            return new ObservableCollection<DecisionTO>(decisions.TheStack.Select(a => new DecisionTO(a)));
        }

        static Dev2DecisionStack SetupTos(ObservableCollection<DecisionTO> value)
        {
           
            var val = new Dev2DecisionStack();
            val.TheStack = new List<Dev2Decision>();
            foreach(var decisionTO in value.Where(a=>!a.IsEmpty()))
            {
                val.TheStack.Add(decisionTO.Decision);
            }
            return val;
        }

        public bool IsDisplayTextFocused { get { return (bool)GetValue(IsDisplayTextFocusedProperty); } set { SetValue(IsDisplayTextFocusedProperty, value); } }
        public static readonly DependencyProperty IsDisplayTextFocusedProperty = DependencyProperty.Register("IsDisplayTextFocused", typeof(bool), typeof(DecisionDesignerViewModel), new PropertyMetadata(default(bool)));

        public bool IsTrueArmFocused { get { return (bool)GetValue(IsTrueArmFocusedProperty); } set { SetValue(IsTrueArmFocusedProperty, value); } }
        public static readonly DependencyProperty IsTrueArmFocusedProperty = DependencyProperty.Register("IsTrueArmFocused", typeof(bool), typeof(DecisionDesignerViewModel), new PropertyMetadata(default(bool)));

        public bool IsFalseArmFocused { get { return (bool)GetValue(IsFalseArmFocusedProperty); } set { SetValue(IsFalseArmFocusedProperty, value); } }
        public static readonly DependencyProperty IsFalseArmFocusedProperty = DependencyProperty.Register("IsFalseArmFocused", typeof(bool), typeof(DecisionDesignerViewModel), new PropertyMetadata(default(bool)));
        ObservableCollection<DecisionTO> _observables;

        void OnSearchTypeChanged(object indexObj)
        {
            var index = (int)indexObj;

            if(index == -1)
            {
                index = 0;
            }

            if(index < 0 || index >= Tos.Count)
            {
                return;
            }

            var mi = Tos[index];

            var searchType = mi.SearchType;

            if(searchType == "Is Between" || searchType == "Not Between")
            {
                mi.IsSearchCriteriaVisible = false;
            }
            else
            {
                mi.IsSearchCriteriaVisible= true;
            }

            var requiresCriteria = _requiresSearchCriteria.Contains(searchType);
            mi.IsSearchCriteriaEnabled= requiresCriteria;
            if(!requiresCriteria)
            {
                mi.SearchCriteria= string.Empty;
            }
        }

        protected override IEnumerable<IActionableErrorInfo> ValidateThis()
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var error in GetRuleSet("DisplayText").ValidateRules("'DisplayText'", () => IsDisplayTextFocused = true))
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                yield return error;
            }
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var error in GetRuleSet("TrueArmText").ValidateRules("'TrueArmText'", () => IsTrueArmFocused = true))
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                yield return error;
            }
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var error in GetRuleSet("FalseArmText").ValidateRules("'FalseArmText'", () => IsFalseArmFocused = true))
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                yield return error;
            }
        }

        protected override IEnumerable<IActionableErrorInfo> ValidateCollectionItem(ModelItem mi)
        {
                yield break;
            }

        //protected override IEnumerable<IActionableErrorInfo> ValidateCollectionItem(DecisionTO mi)
        //{
        //    var dto = mi;
        //    if(dto == null)
        //    {
        //        yield break;
        //    }
        //    //foreach (var error in dto.GetRuleSet("MatchValue", GetDatalistString()).ValidateRules("'Match'", () => mi.IsMatchValueFocused=true)))
        //    //{
        //    //    yield return error;
        //    //}

        //    //foreach (var error in dto.GetRuleSet("SearchCriteria", GetDatalistString()).ValidateRules("'Match'", () => mi.IsSearchCriteriaFocused= true)))
        //    //{
        //    //    yield return error;
        //    //}

        //    //foreach(var error in dto.GetRuleSet("From", GetDatalistString()).ValidateRules("'From'", () => mi.SetProperty("IsFromFocused", true)))
        //    //{
        //    //    yield return error;
        //    //}

        //    //foreach(var error in dto.GetRuleSet("To", GetDatalistString()).ValidateRules("'To'", () => mi.SetProperty("IsToFocused", true)))
        //    //{
        //    //    yield return error;
        //    //}
        //}

        public IRuleSet GetRuleSet(string propertyName)
        {
            var ruleSet = new RuleSet();

            switch(propertyName)
            {
                case "DisplayText":
                    ruleSet.Add(new IsStringEmptyOrWhiteSpaceRule(() => DisplayText));
                    ruleSet.Add(new IsValidExpressionRule(() => DisplayText, GetDatalistString(), "1"));
                    break;

                case "TrueArmText":
                    ruleSet.Add(new IsStringEmptyOrWhiteSpaceRule(() => TrueArmText));
                    ruleSet.Add(new IsValidExpressionRule(() => TrueArmText, GetDatalistString(), "1"));
                    break;

                case "FalseArmText":
                    ruleSet.Add(new IsStringEmptyOrWhiteSpaceRule(() => FalseArmText));
                    ruleSet.Add(new IsValidExpressionRule(() => FalseArmText, GetDatalistString(), "1"));
                    break;
            }
            return ruleSet;
        }

        #region Implementation of IHandle<ConfigureDecisionExpressionMessage>

        public void Handle(ConfigureDecisionExpressionMessage message)
        {
            ShowLarge = true;
        }

        #endregion
    }
}
