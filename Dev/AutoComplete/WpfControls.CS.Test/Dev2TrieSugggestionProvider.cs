using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Gma.DataStructures.StringSearch;
using Microsoft.BusinessData.MetadataModel;
using WpfControls.Editors;

namespace WpfControls.CS.Test
{
    public class Dev2TrieSugggestionProvider:ISuggestionProvider
    {
        #region Implementation of ISuggestionProvider

        public ITrie<string> PatriciaTrie
        {
            get
            {
                return _patriciaTrie;
            }
            private set
            {
                _patriciaTrie = value;
            }
        }
        private long m_WordCount;
        ObservableCollection<string> _variableList;
        int _level;
        IntellisenseStringProvider.FilterOption _filter;
        ITrie<string> _patriciaTrie;
        public ObservableCollection<string> VariableList
        {
            get
            {
                return _variableList;
            }
            set
            {
                _variableList = value;
                PatriciaTrie = new PatriciaTrie<string>();
                var vars = IntellisenseStringProvider.getOptions( _variableList.Select(a => WarewolfDataEvaluationCommon.ParseLanguageExpression(a, 0)),Level,Filter);
                foreach(var @var in vars)
                {
                    PatriciaTrie.Add(@var,@var);  
                }
               
            }
        }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }



        public Dev2TrieSugggestionProvider(IntellisenseStringProvider.FilterOption filter, int level)
        {
            VariableList = new ObservableCollection<string>();
            PatriciaTrie = new PatriciaTrie<string>();
            Level = level;
            Filter = filter;
        }
        public IntellisenseStringProvider.FilterOption Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }

        public IEnumerable GetSuggestions(string filter)
        {
           return PatriciaTrie.Retrieve(filter);
        }

        #endregion
    }
}