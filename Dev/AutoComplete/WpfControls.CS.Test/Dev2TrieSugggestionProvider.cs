using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Gma.DataStructures.StringSearch;
using WpfControls.Editors;

namespace WpfControls.CS.Test
{
    public class Dev2TrieSugggestionProvider:ISuggestionProvider
    {
        #region Implementation of ISuggestionProvider
        private readonly ITrie<string> m_PatriciaTrie;
        private long m_WordCount;
        ObservableCollection<string> _variableList;
        public ObservableCollection<string> VariableList
        {
            get
            {
                return _variableList;
            }
            set
            {
                _variableList = value;
                var vars = IntellisenseStringProvider.Combine( _variableList.Select(a => WarewolfDataEvaluationCommon.ParseLanguageExpression(a, 0)),1);
                foreach(var @var in vars)
                {
                    m_PatriciaTrie.Add(@var,@var);  
                }
               
            }
        }

        public Dev2TrieSugggestionProvider()
        {
            VariableList = new ObservableCollection<string>();
            m_PatriciaTrie = new PatriciaTrie<string>();
            
        }


        


        public IEnumerable GetSuggestions(string filter)
        {
           return m_PatriciaTrie.Retrieve(filter);
        }

        #endregion
    }
}