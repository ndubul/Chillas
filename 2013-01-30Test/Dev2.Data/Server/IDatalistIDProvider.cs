﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev2.DataList.Contract
{
    public interface IDataListIDProvider
    {
        Guid AllocateID();
        bool ValidateID(Guid id);
    }
}
