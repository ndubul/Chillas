using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Reflection;
using Dev2.Communication;
using Dev2.DynamicServices;
using Dev2.DynamicServices.Objects;
using Dev2.Workspaces;

namespace Dev2.Runtime.ESB.Management.Services
{
    public class GetFiles : IEsbManagementEndpoint
    {
        public StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            ExecuteMessage msg = new ExecuteMessage();
            Dev2JsonSerializer serializer = new Dev2JsonSerializer();
            Dev2Logger.Log.Info("Get Files");
            StringBuilder dllListing;

            values.TryGetValue("currentDllListing", out dllListing);
            if (dllListing != null)
            {
                var src = serializer.Deserialize(dllListing.ToString(), typeof(IFileListing)) as IFileListing;
                try
                {
                    msg.HasError = false;
                    msg.Message = serializer.SerializeToBuilder(GetDllListing(src));
                }
                catch (Exception ex)
                {
                    Dev2Logger.Log.Error(ex);
                    msg.HasError = true;
                    msg.SetMessage(ex.Message);
                }
            }

            return serializer.SerializeToBuilder(msg);
        }

        static List<IFileListing> GetDllListing(IFileListing src)
        {
            var completeList = new List<IFileListing>();
            var fileSystemParent = new FileListing { Name = "Computer", IsDirectory = true };
   

            if (src == null)
            {
                var drives = DriveInfo.GetDrives();
                try
                {
                    var listing = drives.Select(BuildDllListing);
                    fileSystemParent.Children = listing.ToList();
                }
                catch (Exception e)
                {
                    Dev2Logger.Log.Error(e.Message);
                }

            }
            else
            {
                if(src.IsDirectory)
                    completeList = GetChildrenForDllListing(new DirectoryInfo(src.FullName));
            }
            return completeList;
        }

        static IFileListing BuildDllListing(DriveInfo info)
        {

            try
            {
                var directory = info.RootDirectory;
                var dllListing = BuildDllListing(directory);
                dllListing.IsDirectory = true;
                return dllListing;
            }
            catch (Exception e)
            {
                Dev2Logger.Log.Error("Error enumerating directory.", e);
            }
            return null;
        }

        static DllListing BuildDllListing(DirectoryInfo directory)
        {
            var dllListing = BuildDllListing(directory as FileSystemInfo);
            try
            {
                dllListing.Children = GetChildrenForDllListing(directory);

            }
            catch (Exception e)
            {
                Dev2Logger.Log.Error("Error enumerating directory.", e);
            }
            return dllListing;
        }

        static List<IFileListing> GetChildrenForDllListing(DirectoryInfo directory)
        {
            var directories = directory.EnumerateDirectories();
            var childList = new List<IFileListing>();
            foreach (var directoryInfo in directories)
            {
                var directoryItem = BuildDllListing((FileSystemInfo)directoryInfo);
                directoryItem.IsDirectory = true;
                childList.Add(directoryItem);
            }
            var files = directory.EnumerateFiles();
            foreach (var fileInfo in files)
            {
                var fileItem = BuildDllListing(fileInfo);
                fileItem.IsDirectory = false;
                childList.Add(fileItem);
            }
            return childList;
        }

        static DllListing BuildDllListing(FileSystemInfo fileInfo)
        {
            var dllListing = new DllListing { Name = fileInfo.Name, FullName = fileInfo.FullName };
            return dllListing;
        }

        public DynamicService CreateServiceEntry()
        {
            DynamicService findDirectoryService = new DynamicService
            {
                Name = HandlesType(),
                DataListSpecification = new StringBuilder("<DataList><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>")
            };

            ServiceAction findDirectoryServiceAction = new ServiceAction
            {
                Name = HandlesType(),
                ActionType = enActionType.InvokeManagementDynamicService,
                SourceMethod = HandlesType()
            };

            findDirectoryService.Actions.Add(findDirectoryServiceAction);

            return findDirectoryService;
        }

        public string HandlesType()
        {
            return "GetDllListingsService";
        }
    }
}