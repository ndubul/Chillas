﻿using Dev2;
using Dev2.Data.Enums;
using Dev2.Interfaces;
using Dev2.Studio.Core.Helpers;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    public class DTOFactory
    {

        public static IDev2TOFn CreateNewDTO(IDev2TOFn dto, int index = 0, bool inserted = false, string initializeWith = "")
        {
            IDev2TOFn toReturn = null;

            TypeSwitch.Do(dto,
                // ReSharper disable ImplicitlyCapturedClosure
            TypeSwitch.Case<ActivityDTO>(x => toReturn = new ActivityDTO(initializeWith, "", index, inserted)),
                // ReSharper restore ImplicitlyCapturedClosure
            TypeSwitch.Case<DataSplitDTO>(x =>
                {
                    var dataSplitDto = dto as DataSplitDTO;
                    if(dataSplitDto != null)
                    {
                        toReturn = new DataSplitDTO(initializeWith, dataSplitDto.SplitType, dataSplitDto.At, index, false, inserted);
                    }
                }),
            TypeSwitch.Case<DataMergeDTO>(x =>
                {
                    var dataMergeDto = dto as DataMergeDTO;
                    if(dataMergeDto != null)
                    {
                        toReturn = new DataMergeDTO(initializeWith, dataMergeDto.MergeType, dataMergeDto.At, index, dataMergeDto.Padding, dataMergeDto.Alignment, inserted);
                    }
                }),
            TypeSwitch.Case<CaseConvertTO>(x =>
                {
                    var caseConvertTO = dto as CaseConvertTO;
                    if(caseConvertTO != null)
                    {
                        toReturn = CaseConverterFactory.CreateCaseConverterTO(initializeWith, caseConvertTO.ConvertType, caseConvertTO.Result, index);
                    }
                }),
            TypeSwitch.Case<BaseConvertTO>(x =>
                {
                    var baseConvertTO = dto as BaseConvertTO;
                    if(baseConvertTO != null)
                    {
                        toReturn = new BaseConvertTO(initializeWith, baseConvertTO.FromType, baseConvertTO.ToType, baseConvertTO.ToExpression, index, inserted);
                    }
                }),
                // ReSharper disable ImplicitlyCapturedClosure
            TypeSwitch.Case<GatherSystemInformationTO>(x => toReturn =
                // ReSharper restore ImplicitlyCapturedClosure
                new GatherSystemInformationTO(enTypeOfSystemInformationToGather.FullDateTime,
                    initializeWith, index, inserted)),
                // ReSharper disable ImplicitlyCapturedClosure
            TypeSwitch.Case<XPathDTO>(x => toReturn = new XPathDTO(initializeWith, "", index, false, inserted)),
                // ReSharper restore ImplicitlyCapturedClosure
                // ReSharper disable ImplicitlyCapturedClosure
            TypeSwitch.Case<FindRecordsTO>(() => toReturn = new FindRecordsTO("", "", index, false, inserted)),
                // ReSharper restore ImplicitlyCapturedClosure
                // ReSharper disable ImplicitlyCapturedClosure
            TypeSwitch.Default(() => toReturn = null));
            // ReSharper restore ImplicitlyCapturedClosure

            return toReturn;
        }
    }
}
