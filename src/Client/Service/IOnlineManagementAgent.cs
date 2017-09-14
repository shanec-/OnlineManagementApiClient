// ———————————————————————–
// <copyright company="Shane Carvalho">
//      Dynamics CRM Online Management API Client
//      Copyright(C) 2017  Shane Carvalho

//      This program is free software: you can redistribute it and/or modify
//      it under the terms of the GNU General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.

//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//      GNU General Public License for more details.

//      You should have received a copy of the GNU General Public License
//      along with this program.If not, see<http://www.gnu.org/licenses/>.
// </copyright>
// ———————————————————————–

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
        Task<OperationStatus> GetOperationStatus(GetOperationStatus getOperationStatusRequest);
    }
}
