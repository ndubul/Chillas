﻿using System;
using System.Activities;
using System.Collections.Generic;
using Dev2;
using Dev2.Activities;
using Dev2.DataList.Contract;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.DataList.Contract.Value_Objects;
using Dev2.Diagnostics;
using Dev2.PathOperations;
using Dev2.Util;
using Unlimited.Applications.BusinessDesignStudio.Activities.Utilities;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{

    /// <summary>
    /// PBI : 1172
    /// Status : New
    /// Purpose : To provide an activity that can un-zip the contents of a zip archive from FTP, FTPS and file system
    /// </summary>
    public class DsfUnZip : DsfAbstractFileActivity, IUnZip, IPathOverwrite, IPathOutput, IPathInput
    {

        public DsfUnZip()
            : base("Unzip")
        {
            ArchivePassword = string.Empty;
            InputPath = string.Empty;
            OutputPath = string.Empty;
        }

        protected override IList<OutputTO> ExecuteConcreteAction(NativeActivityContext context, out ErrorResultTO allErrors)
        {

            IList<OutputTO> outputs = new List<OutputTO>();
            IDSFDataObject dataObject = context.GetExtension<IDSFDataObject>();

            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
            allErrors = new ErrorResultTO();
            ErrorResultTO errors = new ErrorResultTO();
            Guid executionId = dataObject.DataListID;
            IDev2IteratorCollection colItr = Dev2ValueObjectFactory.CreateIteratorCollection();

            //get all the possible paths for all the string variables
            IBinaryDataListEntry inputPathEntry = compiler.Evaluate(executionId, enActionType.User, InputPath, false, out errors);
            allErrors.MergeErrors(errors);
            IDev2DataListEvaluateIterator inputItr = Dev2ValueObjectFactory.CreateEvaluateIterator(inputPathEntry);
            colItr.AddIterator(inputItr);

            IBinaryDataListEntry outputPathEntry = compiler.Evaluate(executionId, enActionType.User, OutputPath, false, out errors);
            allErrors.MergeErrors(errors);
            IDev2DataListEvaluateIterator outputItr = Dev2ValueObjectFactory.CreateEvaluateIterator(outputPathEntry);
            colItr.AddIterator(outputItr);

            IBinaryDataListEntry usernameEntry = compiler.Evaluate(executionId, enActionType.User, Username, false, out errors);
            allErrors.MergeErrors(errors);
            IDev2DataListEvaluateIterator unameItr = Dev2ValueObjectFactory.CreateEvaluateIterator(usernameEntry);
            colItr.AddIterator(unameItr);

            IBinaryDataListEntry passwordEntry = compiler.Evaluate(executionId, enActionType.User, Password, false, out errors);
            allErrors.MergeErrors(errors);
            IDev2DataListEvaluateIterator passItr = Dev2ValueObjectFactory.CreateEvaluateIterator(passwordEntry);
            colItr.AddIterator(passItr);

            IBinaryDataListEntry archPassEntry = compiler.Evaluate(executionId, enActionType.User, ArchivePassword, false, out errors);
            allErrors.MergeErrors(errors);
            IDev2DataListEvaluateIterator archPassItr = Dev2ValueObjectFactory.CreateEvaluateIterator(archPassEntry);
            colItr.AddIterator(archPassItr);

            outputs.Add(DataListFactory.CreateOutputTO(Result));

            if(dataObject.IsDebug || dataObject.RemoteInvoke)
            {
                AddDebugInputItem(InputPath, "Zip Name", inputPathEntry, executionId);
                AddDebugInputItem(OutputPath, "Destination", outputPathEntry, executionId);
                DebugItem itemToAdd = new DebugItem();
                itemToAdd.ResultsList.Add(new DebugItemResult { Type = DebugItemResultType.Label, Value = "Overwrite" });                
                itemToAdd.ResultsList.Add(new DebugItemResult { Type = DebugItemResultType.Value, Value = Overwrite ? "True" : "False"});
                _debugInputs.Add(itemToAdd);
                AddDebugInputItem(Username, "Username", usernameEntry, executionId);
                itemToAdd = new DebugItem();
                itemToAdd.ResultsList.Add(new DebugItemResult { Type = DebugItemResultType.Label, Value = "Password" });                
                itemToAdd.ResultsList.Add(new DebugItemResult { Type = DebugItemResultType.Value, Value = GetBlankedOutPassword(Password) });
                _debugInputs.Add(itemToAdd);
                AddDebugInputItem(ArchivePassword, "Archive Password", archPassEntry, executionId);
            }

            while(colItr.HasMoreData())
            {

                string error = string.Empty;
                IActivityOperationsBroker broker = ActivityIOFactory.CreateOperationsBroker();
                Dev2CRUDOperationTO opTO = new Dev2CRUDOperationTO(Overwrite);

                try
                {
                    Dev2UnZipOperationTO zipTO = ActivityIOFactory.CreateUnzipTO(colItr.FetchNextRow(archPassItr).TheValue);

                    IActivityIOPath src = ActivityIOFactory.CreatePathFromString(colItr.FetchNextRow(inputItr).TheValue,
                                                                                colItr.FetchNextRow(unameItr).TheValue,
                                                                                colItr.FetchNextRow(passItr).TheValue,
                                                                                IsNotCertVerifiable);

                    IActivityIOPath dst = ActivityIOFactory.CreatePathFromString(colItr.FetchNextRow(outputItr).TheValue,
                                                                                    colItr.FetchNextRow(unameItr).TheValue,
                                                                                    colItr.FetchNextRow(passItr).TheValue,
                                                                                    IsNotCertVerifiable);

                    IActivityIOOperationsEndPoint scrEndPoint = ActivityIOFactory.CreateOperationEndPointFromIOPath(src);
                    IActivityIOOperationsEndPoint dstEndPoint = ActivityIOFactory.CreateOperationEndPointFromIOPath(dst);


                    string result = broker.UnZip(scrEndPoint, dstEndPoint, zipTO);
                    outputs[0].OutputStrings.Add(result);

                }
                catch(Exception e)
                {
                    allErrors.AddError(e.Message);
                }
            }

            return outputs;

        }

        #region Properties

        /// <summary>
        /// Gets or sets the archive password.
        /// </summary>      
        [Inputs("Archive Password")]
        [FindMissing]
        public string ArchivePassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DsfUnZip" /> is overwrite.
        /// </summary>
        [Inputs("Overwrite")]
        public bool Overwrite
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        [Inputs("Output Path")]
        [FindMissing]
        public string OutputPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the input path.
        /// </summary>
        [Inputs("Input Path")]
        [FindMissing]
        public string InputPath
        {
            get;
            set;
        }

        #endregion Properties

        #region Private Methods

        private string GetBlankedOutPassword(string password)
        {
            int counter = 0;
            string result = string.Empty;
            while(counter < password.Length)
            {
                result = result + "*";
                counter++;
            }
            return result;
        }   

        #endregion

        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            foreach(Tuple<string, string> t in updates)
            {
                if(t.Item1 == ArchivePassword)
                {
                    ArchivePassword = t.Item2;
                }

                if(t.Item1 == Overwrite.ToString())
                {
                    bool tmpOverwrite;
                    bool.TryParse(t.Item2, out tmpOverwrite);
                    Overwrite = tmpOverwrite;
                }

                if(t.Item1 == InputPath)
                {
                    InputPath = t.Item2;
                }

                if(t.Item1 == OutputPath)
                {
                    OutputPath = t.Item2;
                }
            }
        }

        public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            if(updates.Count == 1)
            {
                Result = updates[0].Item2;
            }
        }

        #region GetForEachInputs/Outputs

        public override IList<DsfForEachItem> GetForEachInputs()
        {
            return GetForEachItems(ArchivePassword, Overwrite.ToString(), OutputPath, InputPath);
        }

        public override IList<DsfForEachItem> GetForEachOutputs()
        {
            return GetForEachItems(Result);
        }

        #endregion

    }
}
