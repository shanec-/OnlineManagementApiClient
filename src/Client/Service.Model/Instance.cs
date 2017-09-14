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
    public class Instance
    {
        public Guid Id { get; set; }
        public string UniqueName { get; set; }
        public string Version { get; set; }
        public string ApplicationUrl { get; set; }
        public string ApiUrl { get; set; }
        public int State { get; set; }
        public bool StateIsSupportedForDelete { get; set; }
        public bool AdminMode { get; set; }
        public int Type { get; set; }
        public object Purpose { get; set; }
        public string FriendlyName { get; set; }
        public string DomainName { get; set; }
        public int BaseLanguage { get; set; }
        public string InitialUserEmail { get; set; }
        public Guid SecurityGroupId { get; set; }
    }

}
