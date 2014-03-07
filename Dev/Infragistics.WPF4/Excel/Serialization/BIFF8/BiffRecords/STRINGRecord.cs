using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;






using Infragistics.Documents.Excel.Serialization.Excel2007;
    
namespace Infragistics.Documents.Excel.Serialization.BIFF8.BiffRecords

{
	internal class STRINGRecord : Biff8RecordBase
	{
		public override void Load( BIFF8WorkbookSerializationManager manager )
		{
			// MD 9/2/08 - Excel formula solving
			// Implemented the loading of this record

			ChildDataItem dataItem = (ChildDataItem)manager.ContextStack[ typeof( ChildDataItem ) ];

			if ( dataItem == null )
			{
                Utilities.DebugFail("The data item was not on the context stack");
				return;
			}

			string value = manager.CurrentRecordStream.ReadFormattedString( LengthType.SixteenBit ).UnformattedString;
			dataItem.Data = value;
		}

		public override void Save( BIFF8WorkbookSerializationManager manager )
		{
			// MD 9/2/08 - Excel formula solving
			// Implemented the saving of this record

			ChildDataItem dataItem = (ChildDataItem)manager.ContextStack[ typeof( ChildDataItem ) ];

			if ( dataItem == null )
			{
                Utilities.DebugFail("The data item was not on the context stack");
				return;
			}

			string value = dataItem.Data as string;

			if ( value == null )
			{
                Utilities.DebugFail("The data item did not contain a string.");
				return;
			}

			manager.CurrentRecordStream.Write( value, LengthType.SixteenBit );
		}

		public override BIFF8RecordType Type
		{
			get { return BIFF8RecordType.STRING; }
		}
	}
}

#region Copyright (c) 2001-2012 Infragistics, Inc. All Rights Reserved
/* ---------------------------------------------------------------------*
*                           Infragistics, Inc.                          *
*              Copyright (c) 2001-2012 All Rights reserved               *
*                                                                       *
*                                                                       *
* This file and its contents are protected by United States and         *
* International copyright laws.  Unauthorized reproduction and/or       *
* distribution of all or any portion of the code contained herein       *
* is strictly prohibited and will result in severe civil and criminal   *
* penalties.  Any violations of this copyright will be prosecuted       *
* to the fullest extent possible under law.                             *
*                                                                       *
* THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
* TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
* TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
* CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
* THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF INFRAGISTICS, INC. *
*                                                                       *
* UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
* PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
* SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY INFRAGISTICS PRODUCT.    *
*                                                                       *
* THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
* CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF INFRAGISTICS,      *
* INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
* INSURE ITS CONFIDENTIALITY.                                           *
*                                                                       *
* THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
* PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
* EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
* THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
* SOURCE CODE CONTAINED HEREIN.                                         *
*                                                                       *
* THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
* --------------------------------------------------------------------- *
*/
#endregion Copyright (c) 2001-2012 Infragistics, Inc. All Rights Reserved