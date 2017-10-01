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

namespace OnlineManagementApiClient.Service.Model
{
    /// <summary>
    /// Restores a Customer Engagement instance in your Office 365 tenant.
    /// </summary>
    public class RestoreInstanceBackupRequest
    {
        /// <summary>
        /// Gets or sets the target instance identifier.
        /// </summary>
        /// <value>
        /// The target instance identifier.
        /// </value>
        /// <remarks>ID of the Customer Engagement instance that you want to restore.</remarks>
        public Guid TargetInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the restore instance backup.
        /// </summary>
        /// <value>
        /// The restore instance backup.
        /// </value>
        public RestoreInstanceBackup RestoreInstanceBackup { get; set; }
    }
}
