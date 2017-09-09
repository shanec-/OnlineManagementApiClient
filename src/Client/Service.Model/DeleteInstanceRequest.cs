using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service.Model
{
    public class DeleteInstanceRequest
    {
        public Guid InstanceId { get; set; }

        [JsonIgnore]
        public bool IsValidateOnlyRequest { get; set; }
    }
}
