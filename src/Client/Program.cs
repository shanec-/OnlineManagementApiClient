using OnlineManagementApiClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IOnlineManagementAgent service = new CrmOnlineManagmentService("https://admin.services.crm6.dynamics.com");

            Task.Run(() =>
                {
                    var instances = service.GetInstances().Result;

                    foreach (var i in instances)
                    {
                        Console.WriteLine($"{i.UniqueName}");
                    }

                    service.DeleteInstance(new Service.Model.DeleteInstanceRequest()
                    {
                        InstanceId = instances.FirstOrDefault().Id
                    });

                    //var serviceVersionId = service.GetServiceVersion().Result;

                    //service.CreateInstance(new Service.Model.CreateInstanceRequest()
                    //{
                    //    ServiceVersionId = serviceVersionId,
                    //    Type = "2",
                    //    BaseLanguage = "1033",
                    //    FriendlyName = "oakton",
                    //    DomainName = "sndxb16",
                    //    InitialUserEmail = "admin@sndbx16.onmicrosoft.com"
                    //});

                })
                .Wait();

            //await service.CreateInstance(new Service.Model.CreateInstanceRequest()
            //{
            //    ServiceVersionId = "31cafafe-c6b1-4c0a-bb53-73927841bc5c",
            //    Type = "2",
            //    BaseLanguage = "1033",
            //    FriendlyName = "oakton",
            //    DomainName = "sndxb16",
            //    InitialUserEmail = "admin@sndbx16.onmicrosoft.com"
            //});
        }
    }
}
