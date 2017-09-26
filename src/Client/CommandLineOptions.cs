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
using CommandLine;

namespace OnlineManagementApiClient
{
    /// <summary>
    /// Shared command line options
    /// </summary>
    public class BaseOptions
    {
        [Option(shortName: 'u', longName: "serviceurl", Default = "https://admin.services.crm6.dynamics.com", Required = false, HelpText = "A valid service url.")]
        public string ServiceUrl { get; set; }
    }

    /// <summary>
    /// Get instances command line options
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetInstance", HelpText = "Retrieve a list of instances.")]
    public class GetInstancesOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the unique name of a specific instance.
        /// </summary>
        /// <value>
        /// The unique name of the instance.
        /// </value>
        [Option(shortName: 'u', longName: "uniquename", Required = false, HelpText = "Retrieve instance with specific unique name.")]
        public string UniqueName { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of a specific instance..
        /// </summary>
        /// <value>
        /// The friendly name of the instance.
        /// </value>
        [Option(shortName: 'f', longName: "friendlyname", Required = false, HelpText = "Retrieve instance with specific friendly name.")]
        public string FriendlyName { get; set; }
    }

    /// <summary>
    /// Create instance command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("CreateInstance", HelpText = "Create new CRM instance.")]
    public class CreateInstanceOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the friendly name of the instance.
        /// </summary>
        /// <value>
        /// The friendly name of the instance.
        /// </value>
        [Option(shortName: 'f', longName: "friendlyname", Required = true, HelpText = "Friendly name for the new instance.")]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        /// <value>
        /// The domain name.
        /// </value>
        [Option(shortName: 'd', longName: "domainname", Required = true, HelpText = "Domain name for the new instance. Ex. https://<domainname>.crm6.dynamics.com")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the initial user email.
        /// </summary>
        /// <value>
        /// The initial user email.
        /// </value>
        [Option(shortName: 'e', longName: "initialuseremail", Required = true, HelpText = "Initial User Email Address.")]
        public string InitialUserEmail { get; set; }

        /// <summary>
        /// Gets or sets the service version name.
        /// </summary>
        /// <value>
        /// The service version name.
        /// </value>
        [Option(shortName: 's', longName: "serviceversionname", Default = "", Required = false, HelpText = "Service version name. If not provided, the first one will be queried from the service.")]
        public string ServiceVersionName { get; set; }

        /// <summary>
        /// Gets or sets the service instance type.
        /// </summary>
        /// <value>
        /// The service instance type.
        /// </value>
        [Option(shortName: 't', longName: "type", Default = 2, HelpText = "The service instance type. Valid options are: 1 = Production, 2 = Sandbox. Defaults to Sandbox.")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the base language.
        /// </summary>
        /// <value>
        /// The base language.
        /// </value>
        [Option(shortName: 'l', longName: "baselanguage", Default = "1033", Required = false, HelpText = "Base language Id. Defaults to English (1033).")]
        public string BaseLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is validation request only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validation request; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'v', longName: "validateonly", Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnly { get; set; }
    }

    /// <summary>
    /// Delete instance command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("DeleteInstance", HelpText = "Delete existing CRM instance.")]
    public class DeleteInstanceOptions : BaseOptions
    {
        [Option(shortName: 'f', longName: "friendlyname", Required = false, HelpText = "The friendly name of the instance to delete.")]
        public string InstanceFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the unique instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        [Option(shortName: 'i', longName: "instanceid", Required = false, HelpText = "The unique identifier of the instance to delete.")]
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is validation request only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validation request; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'v', longName: "validateonly", Default = false, HelpText = "Only validate the request.")]
        public bool ValidateOnly { get; set; }

        /// <summary>
        /// Gets or sets a value confirming the continuation of the delete process.
        /// </summary>
        /// <value>
        ///   <c>true</c> if confirm; otherwise, <c>false</c>.
        /// </value>
        [Option(shortName: 'c', longName: "confirm", Default = false, HelpText = "Confirm the deletion of the instance.")]
        public bool Confirm { get; set; }
    }

    /// <summary>
    /// Get Operation Status command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetOperation", HelpText = "Retrieve the details about an operation.")]
    public class GetOperationStatusOptions : BaseOptions
    {
        /// <summary>
        /// Gets or sets the unique operation identifier.
        /// </summary>
        /// <value>
        /// The operation identifier.
        /// </value>
        [Option(shortName: 'i', longName: "operationid", Required = true, HelpText = "Operation Id.")]
        public Guid OperationId { get; set; }
    }

    /// <summary>
    /// Get Service Versions command line options.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.BaseOptions" />
    [Verb("GetServiceVersion", HelpText = "Get Service Versions.")]
    public class GetServiceVersions : BaseOptions
    {
        [Option(shortName: 'n', longName: "name", Default = "", Required = false, HelpText = "Service version name.")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Get Backup Instances
    /// </summary>
    [Verb("GetInstanceBackups", HelpText = "Retrieve a list of backups for instance.")]
    public class GetInstanceBackupsOptions
    {
        [Option(shortName: 'i', longName: "instanceid", Required = false, HelpText = "The unique identifier of the instance.")]
        public Guid InstanceId { get; set; }
    }

    [Verb("CreateInstanceBackup", HelpText = "Create a backup of an instance.")]
    public class CreateInstanceBackupOptions
    {

    }

    [Verb("RestoreInstanceBackup", HelpText = "Restore a backup of an instance.")]
    public class RestoreInstanceBackupOptions
    {

    }
}
