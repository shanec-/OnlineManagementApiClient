using System;
using System.Threading.Tasks;
using OnlineManagementApiClient.Service;
using OnlineManagementApiClient.Utility;

namespace OnlineManagementApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog logger = new ConsoleLogService(
                new FileLogService("Log.txt"));

            IOnlineManagementAgent service = new CrmOnlineManagmentService(logger, "https://admin.services.crm6.dynamics.com");

            try
            {
                Task.Run(() =>
                    {
                        var instances = service.GetInstances().Result;

                        foreach (var i in instances)
                        {
                            logger.Debug($"Instance - State:{i.State} Type:{i.Type} UniqueName:{i.UniqueName}");
                        }

                        //var operationStatusResult =
                        //    service.DeleteInstance(new Service.Model.DeleteInstanceRequest()
                        //    {
                        //        InstanceId = instances.FirstOrDefault().Id,
                        //        IsValidateOnlyRequest = false
                        //    });


                        //logger.Debug($"OperationId: {operationStatusResult.Result.OperationId }");
                        //logger.Debug($"OperationLocation: {operationStatusResult.Result.OperationLocation }");

                        //var serviceVersionId = service.GetServiceVersion().Result;

                        //logger.Debug($"ServiceVersionId: {serviceVersionId}");

                        //var status = 
                        //    service.CreateInstance(new Service.Model.CreateInstanceRequest()
                        //    {
                        //        ServiceVersionId = serviceVersionId,
                        //        Type = Constants.InstanceType.Sandbox.ToString(),
                        //        BaseLanguage = Constants.Languages.English,
                        //        FriendlyName = "zzz",
                        //        DomainName = "sndxb16",
                        //        InitialUserEmail = "admin@sndbx16.onmicrosoft.com",
                        //        IsValidateOnlyRequest = false
                        //    });

                        //logger.Debug($"OperationId: {status.Result.OperationId }");
                        //logger.Debug($"operationlocation: {status.Result.OperationLocation }");

                    })
                    .Wait();
            }
            catch (Exception ex)
            {
                logger.Error("error", ex);
            }

            Console.Read();
        }
    }
}
