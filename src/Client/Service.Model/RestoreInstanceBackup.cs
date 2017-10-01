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
    public class RestoreInstanceBackup
    {
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        /// <remarks>
        /// The CreatedOn date and time of an existing instance backup to use.
        /// </remarks>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the instance backup identifier.
        /// </summary>
        /// <value>
        /// The instance backup identifier.
        /// </value>
        /// <remarks>
        /// The Id of a legacy instance backup to use. If set, this will be used instead of Label or RestorePoint.
        /// </remarks>
        public Guid InstanceBackupId { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        /// <remarks>
        /// The label of an existing instance backup to use. If set, this will be used instead of RestorePoint.
        /// </remarks>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the source instance identifier.
        /// </summary>
        /// <value>
        /// The source instance identifier.
        /// </value>
        public Guid SourceInstanceId { get; set; }
    }
}
