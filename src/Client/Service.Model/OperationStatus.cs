using System;

namespace OnlineManagementApiClient.Service.Model
{
    public class OperationStatus
    {
        public Guid OperationId { get; set; }
        public string Status { get; set; }
        public string OperationLocation { get; set; }
        public string ResourceLocation { get; set; }
        public ItemDescription[] Errors { get; set; }
        public ItemDescription[] Information { get; set; }
        public Context Context { get; set; }
    }
}
