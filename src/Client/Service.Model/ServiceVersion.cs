﻿using System;

namespace OnlineManagementApiClient.Service.Model
{
    public class ServiceVersion
    {
        public string LocalizedName { get; set; }
        public int LCID { get; set; }
        public string Version { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
