using System;
using System.Threading.Tasks;
using CommandLine;
using OnlineManagementApiClient.Service;
using OnlineManagementApiClient.Utility;

namespace OnlineManagementApiClient
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    ILog logger = new ConsoleLogService(
        //        new FileLogService("Log.txt"));

        //    IOnlineManagementAgent service = new CrmOnlineManagmentService(logger, "https://admin.services.crm6.dynamics.com");

        //    try
        //    {
        //        Task.Run(() =>
        //            {
        //                var instances = service.GetInstances().Result;

        //                foreach (var i in instances)
        //                {
        //                    logger.Debug($"Instance - State:{i.State} Type:{i.Type} UniqueName:{i.UniqueName}");
        //                }

        //                //var operationStatusResult =
        //                //    service.DeleteInstance(new Service.Model.DeleteInstanceRequest()
        //                //    {
        //                //        InstanceId = instances.FirstOrDefault().Id,
        //                //        IsValidateOnlyRequest = false
        //                //    });


        //                //logger.Debug($"OperationId: {operationStatusResult.Result.OperationId }");
        //                //logger.Debug($"OperationLocation: {operationStatusResult.Result.OperationLocation }");

        //                //var serviceVersionId = service.GetServiceVersion().Result;

        //                //logger.Debug($"ServiceVersionId: {serviceVersionId}");

        //                //var status = 
        //                //    service.CreateInstance(new Service.Model.CreateInstanceRequest()
        //                //    {
        //                //        ServiceVersionId = serviceVersionId,
        //                //        Type = Constants.InstanceType.Sandbox.ToString(),
        //                //        BaseLanguage = Constants.Languages.English,
        //                //        FriendlyName = "zzz",
        //                //        DomainName = "sndxb16",
        //                //        InitialUserEmail = "admin@sndbx16.onmicrosoft.com",
        //                //        IsValidateOnlyRequest = false
        //                //    });

        //                //logger.Debug($"OperationId: {status.Result.OperationId }");
        //                //logger.Debug($"operationlocation: {status.Result.OperationLocation }");

        //            })
        //            .Wait();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("error", ex);
        //    }

        //    Console.Read();
        //}

        static void Main(string[] args)
        {
            ILog logger = new ConsoleLogService(
                new FileLogService("Log.txt"));

            System.Diagnostics.Debugger.Launch();

            var customizedParser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = System.Console.Error;
            });

            var result = customizedParser.ParseArguments<GetInstancesOptions, CreateInstanceOptions, DeleteInstanceOptions, OperationStatusOptions>(args);

            result.MapResult(
                (GetInstancesOptions opts) =>
                {
                    IOnlineManagementAgent service = new CrmOnlineManagmentService(logger, opts.ServiceUrl);
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
                    return 0;
                },
                (DeleteInstanceOptions opts) =>
                {
                    return 0;
                },
                (OperationStatusOptions opts) =>
                {
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
