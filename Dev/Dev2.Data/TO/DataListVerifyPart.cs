﻿using System;

namespace Dev2.DataList.Contract
{
    public class DataListVerifyPart : IDataListVerifyPart {

        public string DisplayValue { get; private set; }

        public string Recordset { get; private set;  }

        public string Field { get; private set; }

        public string Description { get; set; }

        public string RecordsetIndex { get; private set; }

        public bool HasRecordsetIndex {

            get{
                return (RecordsetIndex != string.Empty);
            }

        }

        public bool IsScalar {

            get {
                return (Recordset.Length == 0);
            }
        }

        internal DataListVerifyPart(string recordset, string field) : this(recordset, field, string.Empty, string.Empty) { }

        internal DataListVerifyPart(string recordset, string field, bool useRaw) : this(recordset, field, string.Empty, string.Empty, useRaw) { }

        internal DataListVerifyPart(string recordset, string field, string description) : this(recordset, field, description, string.Empty) { }


        internal DataListVerifyPart(string recordset, string field, string description, string index, bool useRaw = false) {
            Recordset = recordset;
            RecordsetIndex = index;

            if(String.IsNullOrEmpty(recordset) && String.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException(string.Format("{0},{1}","recordset","field"),"Please supply a value for recordset or field");
            }
            if (recordset != null) {
                if (recordset.Contains("[") && recordset.Contains("]")) {
                    int start = recordset.IndexOf("(", System.StringComparison.Ordinal);
                    if(start != -1)
                    {
                        Recordset = recordset.Substring(0, (start));
                    }
                    else
                    {
                        Recordset = recordset.Replace("[", "").Replace("]", "");
                    }
                }
            }

            Field = field;
            Description = description;


            if (useRaw)
            {
                if (field.Length > 0)
                {
                    DisplayValue = "[[" + recordset + field + "]]";
                }
                else
                {
                    DisplayValue = "[[" + field + "]]";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Recordset))
                {
                    Recordset = string.Empty;
                    if (field.Contains("(") && !field.Contains(")"))
                    {
                        DisplayValue = "[[" + field;
                    }
                    else
                    {
                        DisplayValue = "[[" + field + "]]";
                    }
                }
                else
                {
                    if (field.Length > 0)
                    {
                        if (recordset != null && (recordset.Contains("(") && recordset.Contains(")")))
                        {
                            string tmp = recordset.Substring(0, recordset.IndexOf("(", System.StringComparison.Ordinal));

                            DisplayValue = "[[" + tmp + "(" + RecordsetIndex + ")." + field + "]]";
                        }
                        else
                        {
                            DisplayValue = "[[" + Recordset + "(" + RecordsetIndex + ")." + field + "]]";
                        }
                    }
                    else
                    {
                        if (recordset != null && (recordset.Contains("(") && recordset.Contains(")")))
                        {
                            string tmp = recordset.Substring(0, recordset.IndexOf("(", System.StringComparison.Ordinal));
                            DisplayValue = "[[" + tmp + "(" + RecordsetIndex + ")]]";
                        }
                        else
                        {
                            DisplayValue = "[[" + Recordset + "(" + RecordsetIndex + ")]]";
                        }
                    }
                }
            }
        }
    }
}
