using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service.Model
{ 

    //https://stackoverflow.com/questions/9819640/ignoring-null-fields-in-json-net


    public class CreateInstanceRequest
    {
        public Guid ServiceVersionId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Templates { get; set; }

        public string Type { get; set; }
        //public string Purpose { get; set; }
        public string FriendlyName { get; set; }
        public string DomainName { get; set; }
        public string BaseLanguage { get; set; }
        public string InitialUserEmail { get; set; }

        [JsonIgnore]
        public bool IsValidateOnlyRequest { get; set; }
    }
}
