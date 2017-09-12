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
