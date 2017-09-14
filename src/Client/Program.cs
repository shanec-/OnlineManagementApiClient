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
using Model = OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient
{
    public class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
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
                GetServiceVersions>(args);

            // Map the command line parameters to the different options 
            result.MapResult(
                (GetInstancesOptions opts) => operations.Process(opts),
                (CreateInstanceOptions opts) => operations.Process(opts),
                (DeleteInstanceOptions opts) => operations.Process(opts),
                (GetOperationStatusOptions opts) => operations.Process(opts),
                (GetServiceVersions opts) => operations.Process(opts),
                errors => 1);
        }

        /// <summary>
        /// Processes the Get instances operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetInstancesOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                var instances = service.GetInstances(opts.UniqueName).Result;
                var instancesCount = instances?.Count();

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
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                Log.Information($"Attempting to retrieve service version ...");
                var availableServiceVersions = service.GetServiceVersion(opts.ServiceVersionName).Result;

                Log.Information($"{availableServiceVersions.Count() } service versions found.");

                foreach (var v in availableServiceVersions)
                {
                    Log.Information("{@v}", v);
                }

                var serviceVersion = availableServiceVersions.FirstOrDefault();
                if (serviceVersion == null)
                {
                    throw new InvalidOperationException("Unable to find any service versions associated with login.");
                }

                Log.Information($"Using service version: {serviceVersion.Name}.");

                Log.Information("Creating new instance...");
                var status =
                    service.CreateInstance(new Model.CreateInstance()
                    {
                        ServiceVersionId = serviceVersion.Id,
                        Type = opts.Type,
                        BaseLanguage = opts.BaseLanguage,
                        FriendlyName = opts.FriendlyName,
                        DomainName = opts.DomainName,
                        InitialUserEmail = opts.InitialUserEmail,
                        IsValidateOnlyRequest = opts.ValidateOnly
                    })
                    .Result;

                Log.Information("Operation completed successfully.");
                Log.Information("Result: {@status}", status);
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
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            if (!opts.ValidateOnly && !opts.Confirm)
            {
                Console.Write($"Are you sure to proceed with deletion of the instance {{{opts.InstanceId}}}?");
                var key = Console.ReadKey();
                if (key.KeyChar != 'Y' || key.KeyChar != 'y')
                {
                    Console.Write(Environment.NewLine);
                    Log.Warning("User aborted deletion.");
                    return 1;
                }
            }

            Task.Run(() =>
            {
                var status = service.DeleteInstance(new Model.DeleteInstance()
                {
                    InstanceId = opts.InstanceId,
                    IsValidateOnlyRequest = opts.ValidateOnly
                }).Result;

                Log.Information("{@status}", status);
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
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                var status = service.GetOperationStatus(new Model.GetOperationStatus()
                {
                    OperationId = opts.OperationId
                }).Result;

                Log.Information("Result: {@status}");
            })
            .Wait();

            return 0;
        }

        /// <summary>
        /// Processes the get service version operation.
        /// </summary>
        /// <param name="opts">The commandline options.</param>
        /// <returns>0 if successfull.</returns>
        private int Process(GetServiceVersions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentRestService(opts.ServiceUrl);

            Task.Run(() =>
            {
                service.GetServiceVersion();
            })
            .Wait();

            return 0;
        }
    }
}