﻿using Dev2;
using Dev2.Activities;
using Dev2.DataList.Contract;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.Diagnostics;
using Dev2.Enums;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using Unlimited.Applications.BusinessDesignStudio.Activities.Utilities;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    public class DsfCountRecordsetActivity : DsfActivityAbstract<string>
    {
        private string _recordsetName;
        private string _countNumber;
        IDev2DataLanguageParser parser = DataListFactory.CreateLanguageParser();

        /// <summary>
        /// Gets or sets the name of the recordset.
        /// </summary>  
        [Inputs("RecordsetName")]
        public string RecordsetName
        {
            get
            {
                return _recordsetName;
            }
            set
            {
                _recordsetName = value;
            }
        }

        //private Variable<string> _recordSetName = new Variable<string>();
        //private Variable<string> _countNumber2 = new Variable<string>();

        //public InOutArgument<string> RecordsetName { get; set; }

        /// <summary>
        /// Gets or sets the count number.
        /// </summary>  
        [Outputs("CountNumber")]
        public string CountNumber
        {
            get
            {
                return _countNumber;
            }
            set
            {
                _countNumber = value;
            }
        }

        //public InOutArgument<string> CountNumber { get; set; }

        public DsfCountRecordsetActivity()
            : base("Count Records")
        {
            RecordsetName = string.Empty;
            CountNumber = string.Empty;
            this.DisplayName = "Count Records";
        }

        //public override void PreConfigureActivity()
        //{
        //    base.PreConfigureActivity();

        //    RecordsetName = new InOutArgument<string>(_recordSetName);
        //    CountNumber = new InOutArgument<string>(_countNumber2);
        //}

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

        }

        protected override void OnExecute(NativeActivityContext context)
        {
            IList<OutputTO> outputs = new List<OutputTO>();
            IDSFDataObject dataObject = context.GetExtension<IDSFDataObject>();

            IDataListCompiler compiler = context.GetExtension<IDataListCompiler>();

            Guid dlID = dataObject.DataListID;
            ErrorResultTO allErrors = new ErrorResultTO();
            ErrorResultTO errors = new ErrorResultTO();
            Guid executionId = compiler.Shape(dlID, enDev2ArgumentType.Input, InputMapping, out errors);
            if (errors.HasErrors())
            {
                allErrors.MergeErrors(errors);
            }

            // Process if no errors
            try
            {
                if (!string.IsNullOrWhiteSpace(RecordsetName))
                {
                    IBinaryDataListEntry recset = compiler.Evaluate(executionId, enActionType.User, RecordsetName, false, out errors);
                    if (errors.HasErrors())
                    {
                        allErrors.MergeErrors(errors);
                    }

                    if (recset != null && recset.Columns != null && CountNumber != string.Empty)
                    {
                        string error;
                        if (recset.FetchRecordAt(1, out error).Count > 0) compiler.Upsert(executionId, CountNumber, recset.FetchLastRecordsetIndex().ToString(), out errors);//2013.01.25: Ashley Lewis Bug 7853 - Added condition to avoid empty recsets counting 1 instead of 0
                        else compiler.Upsert(executionId, CountNumber, "0", out errors);
                        allErrors.MergeErrors(errors);
                    }
                    else if (recset == null || recset.Columns == null)
                    {
                        allErrors.AddError(RecordsetName + " is not a recordset");
                    }
                    else if (CountNumber == string.Empty)
                    {
                        allErrors.AddError("Blank result variable");
                    }

                    compiler.Shape(executionId, enDev2ArgumentType.Output, OutputMapping, out errors);
                    if (errors.HasErrors())
                    {
                        allErrors.MergeErrors(errors);
                    }
                }
                else
                {
                    allErrors.AddError("No recordset given");
                }
            }
            finally
            {
                // now delete executionID
                compiler.DeleteDataListByID(executionId);

                // Handle Errors
                if (allErrors.HasErrors())
                {
                    string err = DisplayAndWriteError("DsfCountRecordsActivity", allErrors);
                    compiler.UpsertSystemTag(dataObject.DataListID, enSystemTag.Error, err, out errors);
                }
            }
        }

        #region Get Debug Inputs/Outputs

        #region GetDebugInputs

        public override IList<IDebugItem> GetDebugInputs(IBinaryDataList dataList)
        {
            var result = new List<IDebugItem>();

            var item = new DebugItem("Recordset", RecordsetName, string.Empty);
            result.Add(item);

            var rs = GetRecordSet(dataList, RecordsetName);

            var idxItr = rs.FetchRecordsetIndexes();
            while (idxItr.HasMore())
            {
                string error;
                var index = idxItr.FetchNextIndex();
                var record = rs.FetchRecordAt(index, out error);
                // ReSharper disable LoopCanBeConvertedToQuery
                foreach (var recordField in record)
                // ReSharper restore LoopCanBeConvertedToQuery
                {
                    result.Add(new DebugItem(index, recordField));
                }
            }

            return result;
        }

        #endregion

        #region GetDebugOutputs

        public override IList<IDebugItem> GetDebugOutputs(IBinaryDataList dataList)
        {
            var result = new List<IDebugItem>();
            var rs = GetRecordSet(dataList, RecordsetName);
            var count = rs.FetchLastRecordsetIndex() - 1;

            result.Add(new DebugItem(null, CountNumber, " = " + count.ToString(CultureInfo.InvariantCulture)));

            return result;
        }

        #endregion


        #endregion Get Inputs/Outputs

        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            if (updates.Count == 1)
            {
                RecordsetName = updates[0].Item2;
            }
        }

        public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            if (updates.Count == 1)
            {
                CountNumber = updates[0].Item2;
            }
        }


        #region GetForEachInputs/Outputs

        public override IList<DsfForEachItem> GetForEachInputs(NativeActivityContext context)
        {
            return GetForEachItems(context, StateType.Before, RecordsetName);
        }

        public override IList<DsfForEachItem> GetForEachOutputs(NativeActivityContext context)
        {
            return GetForEachItems(context, StateType.After, CountNumber);
        }

        #endregion

    }
}
