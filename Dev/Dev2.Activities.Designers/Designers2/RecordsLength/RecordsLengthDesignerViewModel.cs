using System.Activities.Presentation.Model;
using System.Windows;
using Dev2.Activities.Designers2.Core;
using Dev2.Activities.Utils;

namespace Dev2.Activities.Designers2.RecordsLength
{
    public class RecordsLengthDesignerViewModel : ActivityDesignerViewModel
    {
        public RecordsLengthDesignerViewModel(ModelItem modelItem)
            : base(modelItem)
        {
            AddTitleBarHelpToggle();
            RecordsetNameValue = RecordsetName;
        }
        
        public string RecordsetNameValue { get { return (string)GetValue(RecordsetNameValueProperty); } set { SetValue(RecordsetNameValueProperty, value); } }

        public static readonly DependencyProperty RecordsetNameValueProperty =
            DependencyProperty.Register("RecordsetNameValue", typeof(string), typeof(RecordsLengthDesignerViewModel), new PropertyMetadata(null, OnRecordsetNameValueChanged));

        // DO NOT bind to these properties - these are here for convenience only!!!
        string RecordsetName { set { SetProperty(value); } get { return GetProperty<string>(); } }

        static void OnRecordsetNameValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (RecordsLengthDesignerViewModel)d;
            var value = e.NewValue as string;

            if(!string.IsNullOrWhiteSpace(value))
            {
                viewModel.RecordsetName = ActivityDesignerLanuageNotationConverter.ConvertToTopLevelRSNotation(value); 
            }
        }
        
        public override void Validate()
        {
        }
    }
}