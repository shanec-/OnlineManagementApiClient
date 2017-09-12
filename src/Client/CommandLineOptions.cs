using System;
using CommandLine;

namespace OnlineManagementApiClient
{
    [Verb("GetInstances", HelpText = "Retrieve a list of instances.")]
    public class GetInstances
    {
        [Option(Default = "https://admin.services.crm6.dynamics.com", Required = false, HelpText = "A valid service url. Defaults to https://admin.services.crm6.dynamics.com .")]
        public string ServiceUrl { get; set; }
        [Option(Required = false, HelpText = "Instance unique name.")]
        public string UniqueName { get; set; }
    }

    [Verb("CreateInstance", HelpText = "Create new CRM instance.")]
    public class CreateInstanceOptions
    {
        [Option(Default = "", Required = false, HelpText = "Service Version Name. If not provided, the first one will be queried from the service.")]
        public string ServiceVersionName { get; set; }

        // Default to the Sandbox Service Type 
        [Option(Default = 2, HelpText = "The Service Type. Valid options are 1 = Production and 2 = Sandbox. Defaults to Sandbox")]
        public string Type { get; set; }

        [Option(Required = true, HelpText = "Instance Friendly Name.")]
        public string FriendlyName { get; set; }
        [Option(Required = true, HelpText = "Instance Domain Name.")]
        public string DomainName { get; set; }
        [Option(Default = "1033", Required = false, HelpText = "Base Language Id. Defaults to English (1033).")]
        public string BaseLanguage { get; set; }
        [Option(HelpText = "Initial User Email Address.")]
        public string InitialUserEmail { get; set; }

        [Option(Default = false, HelpText = "Validate Only Request")]
        public bool ValidateOnlyRequest { get; set; }
    }

    [Verb("DeleteInstance", HelpText = "Delete existing CRM instance.")]
    public class DeleteInstanceOptions
    {
        [Option(Required = true, HelpText = "The unique identifier of the instance to delete.")]
        public Guid InstanceId { get; set; }
    }

    [Verb("OperationStatus", HelpText = "Retrieve the details about an operation.")]
    public class OperationStatusOptions
    {
        [Option(Required = true, HelpText = "Operation Id.")]
        public Guid OperationId { get; set; }
    }

}
