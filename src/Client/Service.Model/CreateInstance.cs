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
    public class CreateInstance
    {
        public Guid ServiceVersionId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Templates { get; set; }

        public int Type { get; set; }
        //public string Purpose { get; set; }
        public string FriendlyName { get; set; }
        public string DomainName { get; set; }
        public string BaseLanguage { get; set; }
        public string InitialUserEmail { get; set; }

        [JsonIgnore]
        public bool IsValidateOnlyRequest { get; set; }
    }
}
