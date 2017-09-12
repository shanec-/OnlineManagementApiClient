using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient.Service
{
    public interface IOnlineManagementAgent
    {
        Task<IEnumerable<Instance>> GetInstances(string uniqueName = "");
        Task<IEnumerable<ServiceVersion>> GetServiceVersion(string name = "");
        Task<OperationStatus> CreateInstance(CreateInstance createInstanceRequest);
        Task<OperationStatus> DeleteInstance(DeleteInstance deleteInstanceRequest);
        Task<IEnumerable<OperationStatus>> GetOperationStatus(GetOperationStatus getOperationStatusRequest);
    }
}
