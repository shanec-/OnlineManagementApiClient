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
    public class InstanceBackup
    {
        public AzureStorage AzureStorageInformation { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public Guid Id { get; set; }

        public bool IsAzureBackup { get; set; }

        public string Label { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; }

        public string Version { get; set; }
    }
}
