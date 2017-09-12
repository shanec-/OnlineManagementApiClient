using System;
using CommandLine;

namespace OnlineManagementApiClient
{
    [Verb("GetInstances", HelpText = "Retrieve a list of instances.")]
    public class GetInstancesOptions
    {
        [Option(Default = "https://admin.services.crm6.dynamics.com", Required = false, HelpText = "A valid service url.")]
        public string ServiceUrl { get; set; }
        [Option(Required = false, HelpText = "Retrieve instance with specific unique name.")]
        public string UniqueName { get; set; }
    }

    [Verb("CreateInstance", HelpText = "Create new CRM instance.")]
    public class CreateInstanceOptions
    {
        [Option(shortName: 's', longName: "serviceversionname", Default = "", Required = false, HelpText = "Service version name. If not provided, the first one will be queried from the service.")]
        public string ServiceVersionName { get; set; }

        // Default to the Sandbox Service Type 
        [Option(shortName: 't', longName: "type", Default = 2, HelpText = "The Service Type. Valid options are: 1 = Production, 2 = Sandbox. Defaults to Sandbox.")]
        public string Type { get; set; }

        [Option(shortName: 'f', longName: "friendlyname", Required = true, HelpText = "Instance friendly name.")]
        public string FriendlyName { get; set; }

        [Option(shortName: 'd', longName: "domainname", Required = true, HelpText = "Instance domain name.")]
        public string DomainName { get; set; }

        [Option(shortName: 'l', longName: "baselanguage", Default = "1033", Required = false, HelpText = "Base language Id. Defaults to English (1033).")]
        public string BaseLanguage { get; set; }

        [Option(shortName: 'e', longName: "initialuseremail", HelpText = "Initial User Email Address.")]
        public string InitialUserEmail { get; set; }

        [Option(shortName: 'v', longName: "validateonlyrequest", Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnlyRequest { get; set; }
    }

    [Verb("DeleteInstance", HelpText = "Delete existing CRM instance.")]
    public class DeleteInstanceOptions
    {
        [Option(shortName: 'i', Required = true, HelpText = "The unique identifier of the instance to delete.")]
        public Guid InstanceId { get; set; }

        [Option(shortName: 'v', Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnlyRequest { get; set; }
    }

    [Verb("OperationStatus", HelpText = "Retrieve the details about an operation.")]
    public class OperationStatusOptions
    {
        [Option(Required = true, HelpText = "Operation Id.")]
        public Guid OperationId { get; set; }
    }

    [Verb("GetServiceVersions", HelpText = "Get Service Versions.")]
    public class GetServiceVersions
    {
    }
}
