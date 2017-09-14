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
    /// <summary>
    /// Online Management Agent Interface
    /// </summary>
    public interface IOnlineManagementAgent
    {
        /// <summary>
        /// Retrieve a Customer Engagement instance in your Office 365 tenant.
        /// </summary>
        /// <returns>Enumerable list of available instances.</returns>
        Task<IEnumerable<Instance>> GetInstances();

        /// <summary>
        /// Retrieve information about all the supported releases for Customer Engagement.
        /// </summary>
        /// <returns>Enumerable list of service versions.</returns>
        Task<IEnumerable<ServiceVersion>> GetServiceVersion();

        /// <summary>
        /// Provisions (creates) a Customer Engagement instance in your Office 365 tenant.
        /// </summary>
        /// <param name="createInstanceRequest">The create instance request.</param>
        /// <returns>Operation result.</returns>
        Task<OperationStatus> CreateInstance(CreateInstance createInstanceRequest);

        /// <summary>
        /// Deletes a Customer Engagement instance in your Office 365 tenant.
        /// </summary>
        /// <param name="deleteInstanceRequest">The delete instance request.</param>
        /// <returns>Operation result.</returns>
        Task<OperationStatus> DeleteInstance(DeleteInstance deleteInstanceRequest);

        /// <summary>
        /// Retrieves status of an operation in your Customer Engagement instance.
        /// </summary>
        /// <param name="getOperationStatusRequest">The get operation status request.</param>
        /// <returns>Operation result.</returns>
        Task<OperationStatus> GetOperationStatus(GetOperationStatus getOperationStatusRequest);
    }
}
