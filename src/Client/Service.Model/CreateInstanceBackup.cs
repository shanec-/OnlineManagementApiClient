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

using System;
using Newtonsoft.Json;

namespace OnlineManagementApiClient.Service.Model
{
    /// <summary>
    /// Backs up a Customer Engagement instance.
    /// </summary>
    public class CreateInstanceBackup
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AzureStorage AzureStorageInformation { get; set; }

        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        /// <remarks>
        /// ID of the instance to backup.
        /// </remarks>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is azure backup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is azure backup; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Indicates whether you want to backup the instance to Azure Storage.</remarks>
        public bool IsAzureBackup { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        /// <remarks>Label to help identify this backup for future restoration.</remarks>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        /// <remarks>
        /// Notes to help identify this backup for future restoration.
        /// </remarks>
        public string Notes { get; set; }

    }
}
