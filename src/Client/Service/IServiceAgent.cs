using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient.Service
{
    public interface IServiceAgent
    {
        Task<IEnumerable<Instance>> GetInstances(string uniqueName = "");
        Task<Guid> GetServiceVersion(string name = "");
        Task CreateInstance(CreateInstanceRequest request);
        Task DeleteInstance(DeleteInstanceRequest request);
    }
}
