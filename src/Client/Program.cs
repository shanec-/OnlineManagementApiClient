using System;
using System.Threading.Tasks;
using CommandLine;
using OnlineManagementApiClient.Utility;
using Model = OnlineManagementApiClient.Service.Model;
using System.Linq;

namespace OnlineManagementApiClient
{
    public class Program
    {
        readonly ILog _logger;

        public Program()
        {
            // Initialize the log
            _logger = new ConsoleLogService(
                new FileLogService("Log.txt"));
        }

        static void Main(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            var operation = new Program();

            // Initialize the commandline parser
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

            // Map commandline parameters to the different options 
            result.MapResult(
                (GetInstancesOptions opts) => operation.Process(opts),
                (CreateInstanceOptions opts) => operation.Process(opts),
                (DeleteInstanceOptions opts) => operation.Process(opts),
                (GetOperationStatusOptions opts) => operation.Process(opts),
                (GetServiceVersions opts) => operation.Process(opts),
                errors => 1);

        }

        private int Process(GetInstancesOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(_logger, opts.ServiceUrl);

            Task.Run(() =>
            {
                var instances = service.GetInstances(opts.UniqueName).Result;
                foreach (var i in instances)
                {
                    string instance = $"Id: {i.Id}\r\n";
                    instance += $"UniqueName: {i.UniqueName}\r\n";
                    instance += $"Version: {i.Version}\r\n";

                    instance += $"ApplicationUrl: {i.ApplicationUrl}\r\n";
                    instance += $"ApiUrl: {i.ApiUrl}\r\n";
                    instance += $"State: {i.State}\r\n";

                    instance += $"StateIsSupportedForDelete: {i.StateIsSupportedForDelete}\r\n";
                    instance += $"AdminMode: {i.AdminMode}\r\n";
                    instance += $"Type: {i.Type}\r\n";
                    instance += $"Purpose: {i.Purpose}\r\n";
                    instance += $"FriendlyName: {i.FriendlyName}\r\n";
                    instance += $"DomainName: {i.DomainName}\r\n";
                    instance += $"BaseLanguage: {i.BaseLanguage}\r\n";
                    instance += $"InitialUserEmail: {i.InitialUserEmail}\r\n";
                    instance += $"SecurityGroupId: {i.SecurityGroupId}\r\n";

                    _logger.Information(instance);
                }
            })
            .Wait();

            return 0;
        }

        private int Process(CreateInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(_logger, opts.ServiceUrl);

            Task.Run(() =>
            {
                _logger.Information($"Attempting to retrieve service version with name { opts.ServiceVersionName } ...");
                var availableServiceVersions = service.GetServiceVersion(opts.ServiceVersionName).Result;

                _logger.Debug($"{availableServiceVersions.Count() } service versions found.");

                foreach (var v in availableServiceVersions)
                {
                    string serviceInstance = $"Id:{v.Id}\r\n";
                    serviceInstance += $"Name:{v.Name}\r\n";
                    serviceInstance += $"LocalizedName: {v.LocalizedName}\r\n";
                    serviceInstance += $"LCID: {v.LCID}\r\n";
                    serviceInstance += $"Version: {v.Version}";

                    _logger.Information(serviceInstance);
                }

                var serviceVersion = availableServiceVersions.FirstOrDefault();
                if (serviceVersion == null)
                {
                    throw new InvalidOperationException("Unable to find any service versions associated.");
                }

                // service instance found
                // update the trace here

                // kick off the create instance
                var status =
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

                this.WriteOperationStatusToLog(status);
            })
            .Wait();

            return 0;
        }

        private int Process(DeleteInstanceOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(_logger, opts.ServiceUrl);

            Task.Run(() =>
            {
                var status = service.DeleteInstance(new Model.DeleteInstance()
                {
                    InstanceId = opts.InstanceId,
                    IsValidateOnlyRequest = opts.ValidateOnlyRequest
                }).Result;

                this.WriteOperationStatusToLog(status);
            })
            .Wait();

            return 0;
        }

        private int Process(GetOperationStatusOptions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(_logger, opts.ServiceUrl);

            Task.Run(() =>
            {
                var status = service.GetOperationStatus(new Model.GetOperationStatus()
                {
                    OperationId = opts.OperationId
                }).Result;

                this.WriteOperationStatusToLog(status);
            })
            .Wait();

            return 0;
        }

        private int Process(GetServiceVersions opts)
        {
            Service.IOnlineManagementAgent service =
                        new Service.CrmOnlineManagmentService(_logger, opts.ServiceUrl);

            Task.Run(() =>
            {
                service.GetServiceVersion();
            })
            .Wait();

            return 0;
        }

        private void WriteOperationStatusToLog(Model.OperationStatus status)
        {
            // print out the status here
            string message = $"OperationId: {status.OperationId}\r\n";
            message += $"Status: {status.Status}\r\n";
            message += $"OperationLocation: {status.OperationLocation}\r\n";
            message += $"ResourceLocation: {status.ResourceLocation}\r\n";

            message += "Error:";
            foreach (var e in status.Errors)
            {
                message += $"{{Subject: {e.Subject}, Description: {e.Description}}}";
            }

            message += "Information:";
            foreach (var e in status.Information)
            {
                message += $"{{Subject: {e.Subject}, Description: {e.Description}}}";
            }

            var items = status?.Context?.Items;
            if (items != null)
            {
                message += "Items: ";
                foreach (var i in items)
                {
                    message += $"{{Key:{i.Key}, Value:{i.Value}}}";
                }
            }

            message += "\r\n";

            _logger.Information(message);
        }
    }
}


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
