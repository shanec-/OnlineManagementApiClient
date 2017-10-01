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
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Serilog;
using Serilog.Formatting.Json;
using Model = OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            // initialize the logger using the app config
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            var operations = new Program();

            // initialize the commandline parser
            var customizedParser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = System.Console.Error;
            });

            var result = customizedParser.ParseArguments<
                GetInstancesOptions,
                CreateInstanceOptions,
                DeleteInstanceOptions,
                GetOperationStatusOptions,
                GetServiceVersions,
                GetInstanceBackupsOptions,
                CreateInstanceBackupOptions,
                RestoreInstanceBackupOptions>(args);

            Log.Debug("Input parameters: {@result}", result);

            try
            {
                // Map the command line parameters to the different options 
                result.MapResult(
                    (GetInstancesOptions opts) => operations.Process(opts),
                    (CreateInstanceOptions opts) => operations.Process(opts),
                    (DeleteInstanceOptions opts) => operations.Process(opts),
                    (GetOperationStatusOptions opts) => operations.Process(opts),
                    (GetServiceVersions opts) => operations.Process(opts),
                    (GetInstanceBackupsOptions opts) => operations.Process(opts),
                    (CreateInstanceBackupOptions opts) => operations.Process(opts),
                    (RestoreInstanceBackupOptions opts) => operations.Process(opts),
                    errors => 1);
            }
            catch (AggregateException aEx)
            {
                foreach (var ex in aEx.Flatten().InnerExceptions)
                {
                    if (ex is Microsoft.IdentityModel.Clients.ActiveDirectory.AdalServiceException)
                    {
                        Log.Error(ex.Message);
                        Log.Debug(ex, ex.Message);
                    }
                    else
                    {
                        Log.Error(ex.Message);
                        Log.Debug(ex, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error");
            }
        }

        /// <summary>
        /// Processes the Get instances operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetInstancesOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                var instances = service.GetInstances().Result;
                var instancesCount = instances?.Count();

                if (!string.IsNullOrEmpty(opts.FriendlyName))
                {
                    instances = instances
                        .Where(x => x.FriendlyName.Equals(opts.FriendlyName, StringComparison.InvariantCultureIgnoreCase));
                }
                else if (!string.IsNullOrEmpty(opts.UniqueName))
                {
                    instances = instances.Where(x => x.UniqueName.Equals(opts.UniqueName, StringComparison.InvariantCultureIgnoreCase));
                }

                Log.Information("{@instancesCount} instances found.", instancesCount);

                foreach (var i in instances)
                {
                    Log.Information("{@i}", i);
                }
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the create instance operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(CreateInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                Log.Information($"Attempting to retrieve service version ...");

                var availableServiceVersions = service.GetServiceVersion().Result;
                Log.Information($"{availableServiceVersions.Count() } service versions available.");

                // resolve the service version that is being used.
                Model.ServiceVersion serviceVersion = null;

                // ensure that a specific service version name has been specified.
                if (!string.IsNullOrEmpty(opts.ServiceVersionName))
                {
                    serviceVersion = availableServiceVersions
                        .Where(x => x.Name.Equals(opts.ServiceVersionName, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();
                }
                else
                {
                    serviceVersion = availableServiceVersions.FirstOrDefault();
                }

                if (serviceVersion == null)
                {
                    throw new InvalidOperationException("Unable to find any service versions associated with login.");
                }

                Log.Information($"Using service version: {serviceVersion.Name}.");

                var c = new Model.CreateInstance()
                {
                    ServiceVersionId = serviceVersion.Id,
                    Type = opts.Type,
                    BaseLanguage = opts.BaseLanguage,
                    FriendlyName = opts.FriendlyName,
                    DomainName = opts.DomainName,
                    InitialUserEmail = opts.InitialUserEmail,
                    IsValidateOnlyRequest = opts.ValidateOnly
                };

                Log.Information("Creating new instance with parameters ({@c}) ...", c);

                var status = service.CreateInstance(c).Result;

                this.WriteLog(status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the delete instance operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(DeleteInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            if (!opts.ValidateOnly && !opts.Confirm)
            {
                if (opts.InstanceId != Guid.Empty)
                {
                    Log.Information($"Are you sure to proceed with deletion of the instance [{opts.InstanceId}]?");
                }
                else
                {
                    Log.Information($"Are you sure to proceed with deletion of the instance [{opts.InstanceFriendlyName}]?");
                }

                var key = Console.ReadKey();
                if (key.KeyChar != 'Y' && key.KeyChar != 'y')
                {
                    Console.Write(Environment.NewLine);
                    Log.Warning("User aborted deletion of instance. Exiting.");
                    return 1;
                }

                Console.Write(Environment.NewLine);
            }

            Task.Run(() =>
            {
                Guid? instanceId = Guid.Empty;
                if (opts.InstanceId != Guid.Empty)
                {
                    instanceId = opts.InstanceId;
                }
                else if (!string.IsNullOrEmpty(opts.InstanceFriendlyName))
                {
                    Log.Debug($"Attempting to retrieve instance from instance friendly name: {opts.InstanceFriendlyName}");
                    var instances = service.GetInstances().Result;
                    instanceId = instances?
                        .Where(x => x.FriendlyName.Equals(opts.InstanceFriendlyName, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault()?.Id;
                }
                else
                {
                    throw new ArgumentException("Instance Id or instance friendly name not provided.");
                }

                if (!instanceId.HasValue || instanceId == Guid.Empty)
                {
                    throw new InvalidOperationException("Unable to resolve unique instance identifier.");
                }

                Log.Debug($"Instance Id resolved: {opts.InstanceId}");

                var status = service.DeleteInstance(new Model.DeleteInstance()
                {
                    InstanceId = instanceId.Value,
                    IsValidateOnlyRequest = opts.ValidateOnly
                }).Result;

                this.WriteLog(status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the get operation status operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetOperationStatusOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                var status = service.GetOperationStatus(new Model.GetOperationStatusRequest()
                {
                    OperationId = opts.OperationId
                }).Result;

                this.WriteLog(status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the get service version operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successful.</returns>
        private int Process(GetServiceVersions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                service.GetServiceVersion();
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the retrieval of available instance backups.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successful.</returns>
        private int Process(GetInstanceBackupsOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                var backupInstances = service.GetInstanceBackups(new Model.GetInstanceBackupsRequest() { InstanceId = opts.InstanceId }).Result;
                var backupInstancesCount = backupInstances?.Count();

                Log.Information("{@instancesCount} backups found.", backupInstancesCount);

                foreach (var i in backupInstances)
                {
#warning minimize the verbosity
                    Log.Information("{@i}", i);
                    Log.Debug("{@i}", i);
                }
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the create instance of backup operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successful.</returns>
        private int Process(CreateInstanceBackupOptions opts)
        {

            Service.IOnlineManagementAgent service =
                       new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            Task.Run(() =>
            {
                var status = service.CreateInstanceBackup(
                    new Model.CreateInstanceBackupRequest()
                    {
                        CreateInstanceBackup = new Model.CreateInstanceBackup()
                        {
                            InstanceId = opts.InstanceId,
                            Label = opts.Label,
                            IsAzureBackup = opts.IsAzureBackup,
                            Notes = opts.Notes
                        }
                    }).Result;

                this.WriteLog(status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the restoration of an instance backup operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successful.</returns>
        private int Process(RestoreInstanceBackupOptions opts)
        {
            Service.IOnlineManagementAgent service =
                       new Service.CrmOnlineManagmentRestService(opts.ServiceUrl, opts.Username, opts.Password);

            if (string.IsNullOrEmpty(opts.Label) && opts.InstanceBackupId == Guid.Empty)
            {
                throw new ArgumentException("Label or backup id not provided.");
            }

            Task.Run(() =>
            {
                var status = service.RestoreInstanceBackup(
                    new Model.RestoreInstanceBackupRequest()
                    {
                        TargetInstanceId = opts.TargetInstanceId,
                        RestoreInstanceBackup = new Model.RestoreInstanceBackup()
                        {
                            InstanceBackupId = opts.InstanceBackupId,
                            Label = opts.Label,
                            CreatedOn = opts.CreatedOn,
                            SourceInstanceId = opts.SourceInstanceId
                        }
                    }).Result;

                this.WriteLog(status);
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Writes the oepration status to log.
        /// </summary>
        /// <param name="status">The status.</param>
        private void WriteLog(Model.OperationStatusResponse status)
        {
            var operationStatus = status.OperationStatus;

            if (operationStatus.Errors.Any())
            {
                Log.Error("Error(s) while making request...");

                foreach (var e in operationStatus.Errors)
                {
                    Log.Error($"[{e.Subject}]:{ e.Description}");
                }

                Log.Debug("Result: {@operationStatus}", operationStatus);
            }
            else
            {
                Log.Information("Result: {@operationStatus}", operationStatus);
            }
        }
    }
}