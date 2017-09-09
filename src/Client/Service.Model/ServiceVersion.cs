using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service.Model
{

    public class ServiceVersionResult
    {
        public ServiceVersion[] ServiceVersions { get; set; }
    }

    public class ServiceVersion
    {
        public string LocalizedName { get; set; }
        public int LCID { get; set; }
        public string Version { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}
