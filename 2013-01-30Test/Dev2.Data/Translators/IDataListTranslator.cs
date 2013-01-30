﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dev2.Common;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.DataList.Contract.TO;


namespace Dev2.DataList.Contract
{
    /// <summary>
    /// Responsible for converting to and from various formats
    /// </summary>
    public interface IDataListTranslator //: ISpookyLoadable
    {

        /// <summary>
        /// The format this IDataListTranslator supports.
        /// </summary>
        DataListFormat Format { get; }
        /// <summary>
        /// The text encoding (if any) that this IDataListTranslator uses to work with the binary representations.
        /// </summary>
        Encoding TextEncoding { get; }

        /// <summary>
        /// Converts from a binary representation in the standard format to the specified <see cref="Format" />.
        /// </summary>
        /// <param name="input">The binary representation of the datalist.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>
        /// An array of bytes that represent the datalist in the specified <see cref="Format" />
        /// </returns>
        DataListTranslatedPayloadTO ConvertFrom(IBinaryDataList input, out ErrorResultTO errors);

        /// <summary>
        /// Converts from a binary representation in the specified <see cref="Format"/> to the standard
        /// binary representation of a datalist.
        /// </summary>
        /// <param name="input">The binary representation in the specifeid <see cref="Format"/></param>
        /// <returns>An array of bytes that represent the datalist in the standard format.</returns>
        IBinaryDataList ConvertTo(byte[] input, string shape, out ErrorResultTO errors);

    }
}
