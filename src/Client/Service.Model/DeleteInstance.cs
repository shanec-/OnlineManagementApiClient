using System;
using Newtonsoft.Json;

namespace OnlineManagementApiClient.Service.Model
{
    public class DeleteInstance
    {
        public Guid InstanceId { get; set; }

        [JsonIgnore]
        public bool IsValidateOnlyRequest { get; set; }
    }
}
