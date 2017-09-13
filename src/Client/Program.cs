using System;
using System.Threading.Tasks;
using CommandLine;
using OnlineManagementApiClient.Utility;
using Model = OnlineManagementApiClient.Service.Model;
using System.Linq;

namespace OnlineManagementApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the log
            ILog logger = new ConsoleLogService(
                new FileLogService("Log.txt"));

#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            // Initialize the commandline parser
            var customizedParser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = System.Console.Error;
            });

            var result = customizedParser.ParseArguments<GetInstancesOptions,
                CreateInstanceOptions,
                DeleteInstanceOptions,
                GetOperationStatusOptions,
                GetServiceVersions>(args);

            // Map commandline parameters to the different options 
            result.MapResult(
                (GetInstancesOptions opts) =>
                {
                    Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(logger, opts.ServiceUrl);

                    Task.Run(() =>
                    {
                        var instances = service.GetInstances(opts.UniqueName).Result;
                        foreach (var i in instances)
                        {
                            string instance = $"Id: {i.Id}\n";
                            instance += $"UniqueName: {i.UniqueName}\n";
                            instance += $"Version: {i.Version}\n";

                            instance += $"ApplicationUrl: {i.ApplicationUrl}\n";
                            instance += $"ApiUrl: {i.ApiUrl}\n";
                            instance += $"State: {i.State}\n";

                            instance += $"StateIsSupportedForDelete: {i.StateIsSupportedForDelete}\n";
                            instance += $"AdminMode: {i.AdminMode}\n";
                            instance += $"Type: {i.Type}\n";
                            instance += $"Purpose: {i.Purpose}\n";
                            instance += $"FriendlyName: {i.FriendlyName}\n";
                            instance += $"DomainName: {i.DomainName}\n";
                            instance += $"BaseLanguage: {i.BaseLanguage}\n";
                            instance += $"InitialUserEmail: {i.InitialUserEmail}\n";
                            instance += $"SecurityGroupId: {i.SecurityGroupId}\n";

                            logger.Information(instance);
                        }
                    })
                    .Wait();

                    return 0;
                },
                (CreateInstanceOptions opts) =>
                {
                    Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(logger, opts.ServiceUrl);

                    Task.Run(() =>
                    {
                        logger.Information($"Attempting to retrieve service version with name { opts.ServiceVersionName } ...");
                        var availableServiceVersions = service.GetServiceVersion(opts.ServiceVersionName).Result;

                        logger.Debug($"{availableServiceVersions.Count() } service versions found.");

                        foreach (var v in availableServiceVersions)
                        {
                            string serviceInstance = $"Id:{v.Id}\n";
                            serviceInstance += $"Name:{v.Name}\n";
                            serviceInstance += $"LocalizedName: {v.LocalizedName}\n";
                            serviceInstance += $"LCID: {v.LCID}\n";
                            serviceInstance += $"Version: {v.Version}";

                            logger.Information(serviceInstance);
                        }

                        var serviceVersion = availableServiceVersions.FirstOrDefault();
                        if (serviceVersion == null)
                        {
                            throw new InvalidOperationException("Unable to find any service versions associated.");
                        }

                        // service instance found
                        // update the trace here

                        // kick off the create instance
                        var createInstanceStatus =
                            service.CreateInstance(new Model.CreateInstance()
                            {
                                ServiceVersionId = serviceVersion.Id,
                                Type = opts.Type,
                                BaseLanguage = opts.BaseLanguage,
                                FriendlyName = opts.FriendlyName,
                                DomainName = opts.DomainName,
                                InitialUserEmail = opts.InitialUserEmail,
                                IsValidateOnlyRequest = opts.ValidateOnlyRequest
                            }).Result;

#warning refactor out the printing of status operation to its own method.

                        // print out the status here
                        string createStatusMessage = $"OperationId: {createInstanceStatus.OperationId}\n";
                        createStatusMessage += $"Status: {createInstanceStatus.Status}\n";
                        createStatusMessage += $"OperationLocation: {createInstanceStatus.OperationLocation}\n";
                        createStatusMessage += $"ResourceLocation: {createInstanceStatus.ResourceLocation}\n";
                        foreach (var e in createInstanceStatus.Errors)
                        {
                            createStatusMessage += $"Error: Subject: {e.Subject}, Description: {e.Description}";
                        }

                        foreach (var e in createInstanceStatus.Information)
                        {
                            createStatusMessage += $"Information: Subject: {e.Subject}, Description: {e.Description}";
                        }

                        foreach (var i in createInstanceStatus?.Context?.Items?.Values)
                        {
                            createStatusMessage += $"Item Values {i.ToString()}";
                        }
                    })
                    .Wait();

                    return 0;
                },
                (DeleteInstanceOptions opts) =>
                {
                    Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(logger, opts.ServiceUrl);

                    Task.Run(() =>
                    {
                        service.DeleteInstance(new Model.DeleteInstance()
                        {
                            InstanceId = opts.InstanceId,
                            IsValidateOnlyRequest = opts.ValidateOnlyRequest
                        });
                    })
                    .Wait();

                    return 0;
                },
                (GetOperationStatusOptions opts) =>
                {
                    Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(logger, opts.ServiceUrl);

                    Task.Run(() =>
                    {
                        service.GetOperationStatus(new Model.GetOperationStatus()
                        {
                            OperationId = opts.OperationId
                        });
                    })
                    .Wait();


                    return 0;
                },
                (GetServiceVersions opts) =>
                {
                    Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(logger, opts.ServiceUrl);

                    Task.Run(() =>
                    {
                        service.GetServiceVersion();
                    })
                    .Wait();

                    return 0;
                },
                errors => 1);

            //    try
            //    {
            //        Task.Run(() =>
            //        {
            //            var instances = service.GetInstances().Result;

            //            foreach (var i in instances)
            //            {
            //                logger.Debug($"Instance - State:{i.State} Type:{i.Type} UniqueName:{i.UniqueName}");
            //            }

            //            //var operationStatusResult =
            //            //    service.DeleteInstance(new Service.Model.DeleteInstanceRequest()
            //            //    {
            //            //        InstanceId = instances.FirstOrDefault().Id,
            //            //        IsValidateOnlyRequest = false
            //            //    });


            //            //logger.Debug($"OperationId: {operationStatusResult.Result.OperationId }");
            //            //logger.Debug($"OperationLocation: {operationStatusResult.Result.OperationLocation }");

            //            //var serviceVersionId = service.GetServiceVersion().Result;

            //            //logger.Debug($"ServiceVersionId: {serviceVersionId}");

            //            //var status = 
            //            //    service.CreateInstance(new Service.Model.CreateInstanceRequest()
            //            //    {
            //            //        ServiceVersionId = serviceVersionId,
            //            //        Type = Constants.InstanceType.Sandbox.ToString(),
            //            //        BaseLanguage = Constants.Languages.English,
            //            //        FriendlyName = "zzz",
            //            //        DomainName = "sndxb16",
            //            //        InitialUserEmail = "admin@sndbx16.onmicrosoft.com",
            //            //        IsValidateOnlyRequest = false
            //            //    });

            //            //logger.Debug($"OperationId: {status.Result.OperationId }");
            //            //logger.Debug($"operationlocation: {status.Result.OperationLocation }");

            //        })
            //            .Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error("error", ex);
            //    }

            //    Console.Read();
            //}
        }
    }
}
