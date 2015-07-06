
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
//using System.Text;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Dev2.Activities.Designers2.Core;
using Dev2.Common;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Common.Interfaces.Infrastructure.Providers.Validation;
using Dev2.Communication;
using Dev2.Data.Decisions.Operations;
using Dev2.Data.SystemTemplates;
using Dev2.Data.SystemTemplates.Models;
using Dev2.Data.Util;
using Dev2.DataList;
using Dev2.Interfaces;
using Dev2.Providers.Validation.Rules;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Messages;
using Dev2.TO;
using Dev2.Validation;

namespace Dev2.Activities.Designers2.Decision
{
    public class DecisionDesignerViewModel : ActivityCollectionDesignerObservableViewModel<DecisionTO>
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
            ConfigureDecisionExpression(ModelItem);
            InitializeItems(new ObservableCollection<IDev2TOFn>(Tos));
  
        }

        void ConfigureDecisionExpression(ModelItem mi)
        {

            var condition = mi;
            var expression = condition.Properties[GlobalConstants.ExpressionPropertyText];
            var ds = DataListConstants.DefaultStack;

            if(expression != null && expression.Value != null)
            {
                //we got a model, push it in to the Model region ;)
                // but first, strip and extract the model data ;)

                var eval = Dev2DecisionStack.ExtractModelFromWorkflowPersistedData(expression.Value.ToString());

                if(!string.IsNullOrEmpty(eval))
                {
                    ExpressionText = eval;
                }
            }
            else
            {
                Dev2JsonSerializer ser = new Dev2JsonSerializer();
                ExpressionText = ser.Serialize(ds);
            }

            var displayName = mi.Properties[GlobalConstants.DisplayNamePropertyText];
            if (displayName != null && displayName.Value != null)
            {
                ds.DisplayText = displayName.Value.ToString();
            }
            Tos = ToObservableCollection();
        }


        public void GetExpressionText()
        {
            //IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
            var stack = SetupTos(_observables);
            stack.Mode = RequireAllDecisionsToBeTrue ? Dev2DecisionMode.AND : Dev2DecisionMode.OR;
            stack.DisplayText = DisplayText;
            stack.FalseArmText = FalseArmText;
            stack.TrueArmText = TrueArmText;
            ExpressionText = DataListUtil.ConvertModelToJson(stack).ToString();
        }

        public override string CollectionName { get { return "ResultsCollection"; } }

        public ICommand SearchTypeUpdatedCommand { get; private set; }

        public ObservableCollection<string> WhereOptions { get; private set; }

        public string DisplayText { get; set; }
        public string TrueArmText { get; set; }
        public string FalseArmText { get; set; }
        public string ExpressionText { get; set; }
        public bool RequireAllDecisionsToBeTrue
        {
            get; set;
        }
        public ObservableCollection<DecisionTO> Tos
        {

            get
            {
                return  _observables;
            }
            set
            {
               
                _observables = value;
                var stack = SetupTos(value);
                ExpressionText = DataListUtil.ConvertModelToJson(stack).ToString();
            }
        }

        ObservableCollection<DecisionTO> ToObservableCollection()
        {

            if (!String.IsNullOrWhiteSpace(ExpressionText))
            {
                var val = new StringBuilder(ExpressionText);
                var decisions  = DataListUtil.ConvertFromJsonToModel<Dev2DecisionStack>(val);
                if (decisions != null)
                {
                    if (decisions.TheStack != null)
                    {
                        return new ObservableCollection<DecisionTO>(decisions.TheStack.Select(a => new DecisionTO(a)));
                    }
                }
            }
            return new ObservableCollection<DecisionTO>{new DecisionTO()};
        }

        static Dev2DecisionStack SetupTos(ObservableCollection<DecisionTO> value)
        {
           
            var val = new Dev2DecisionStack();
            val.TheStack = new List<Dev2Decision>();
            foreach(var decisionTO in value.Where(a=>!a.IsEmpty()))
            {
                var dev2Decision = new Dev2Decision();
                dev2Decision.Col1 = decisionTO.MatchValue;
                if(!String.IsNullOrEmpty(decisionTO.SearchCriteria))
                {
                    dev2Decision.Col2 = decisionTO.SearchCriteria;
                }
                dev2Decision.EvaluationFn = DecisionDisplayHelper.GetValue(decisionTO.SearchType);
                if(!String.IsNullOrEmpty(decisionTO.From) || !String.IsNullOrEmpty(decisionTO.To))
                {
                    dev2Decision.Col2 = decisionTO.From;
                    dev2Decision.Col3 = decisionTO.To;
                }
                val.TheStack.Add(dev2Decision);
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

        protected override IEnumerable<IActionableErrorInfo> ValidateCollectionItem(IDev2TOFn mi)
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
