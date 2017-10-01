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

namespace OnlineManagementApiClient.Service.Model
{
    /// <summary>
    /// Azure Storage Information
    /// </summary>
    public class AzureStorage
    {
        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>
        /// The name of the container.
        /// </value>
        /// <remarks>
        /// Container in Azure Storage where you want to back up the instance.
        /// </remarks>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the storage account key.
        /// </summary>
        /// <value>
        /// The storage account key.
        /// </value>
        /// <remarks>
        /// Azure Storage account key to authenticate to the file share for backing up.
        /// </remarks>
        public string StorageAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the storage account.
        /// </summary>
        /// <value>
        /// The name of the storage account.
        /// </value>
        /// <remarks>
        /// Azure Storage account name where want to back up the instance.</remarks>
        public string StorageAccountName { get; set; }
    }
}
