using System;
using Newtonsoft.Json;

namespace OnlineManagementApiClient.Service.Model
{
    public class CreateInstance
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
