using System.Collections.Generic;

namespace Dev2.Common.Interfaces
{
    public interface IManagePluginSourceModel
    {
        string ServerName { get; }
        IList<IDllListing> GetDllListings(IDllListing listing);
        void Save(IPluginSource toDbSource);
    }
}
