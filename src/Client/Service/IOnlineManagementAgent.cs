using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient.Service
{
    public interface IOnlineManagementAgent
    {
        Task<IEnumerable<Instance>> GetInstances(string uniqueName = "");
        Task<Guid> GetServiceVersion(string name = "");
        Task<OperationStatus> CreateInstance(CreateInstanceRequest createInstanceRequest);
        Task<OperationStatus> DeleteInstance(DeleteInstanceRequest deleteInstanceRequest);
        Task<IEnumerable<OperationStatus>> GetOperationStatus(GetOperationStatusRequest getOperationStatusRequest);
    }
}
