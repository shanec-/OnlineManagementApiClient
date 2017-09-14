﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient.Service
{
    public class CrmOnlineManagmentRestService : IOnlineManagementAgent, IDisposable
    {
        private HttpClient _httpClient;
        private string _serviceUrl;

        public CrmOnlineManagmentRestService(string serviceUrl)
        {
            this._serviceUrl = serviceUrl;
        }

        public CrmOnlineManagmentRestService(string serviceUrl, HttpClient httpClient)
        {
            this._serviceUrl = serviceUrl;
            this._httpClient = httpClient;
        }

        public async Task<IEnumerable<Instance>> GetInstances(string uniqueName = "")
        {
            IEnumerable<Instance> result = null;

            this.ConnectToApi();

            var myRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/instances");
            var myResponse = await _httpClient.SendAsync(myRequest);

            if (myResponse.IsSuccessStatusCode)
            {
                var rawResult = myResponse.Content.ReadAsStringAsync().Result;

                Trace.TraceInformation($"Your instances retrieved from Office 365 tenant: \n{rawResult}");

                var res = this.ParseArray<Instance>(rawResult);

                if (!string.IsNullOrEmpty(uniqueName))
                {
                    result = res.Where(x => x.UniqueName == uniqueName);
                }
                else
                {
                    result = res;
                }
            }
            else
            {
                if (myResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result = new List<Instance>();
                }

                Trace.TraceError($"The request failed with a status of '{ myResponse.ReasonPhrase}'");
            }

            return result;
        }

        public async Task<OperationStatus> CreateInstance(CreateInstance request)
        {
            OperationStatus result = null;

            this.ConnectToApi();

            string requestUrl = "/api/v1/instances/Provision/";
            if (request.IsValidateOnlyRequest)
            {
                requestUrl += "?validate=true";
            }

            HttpRequestMessage myRequest = new HttpRequestMessage(HttpMethod.Put, requestUrl);

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            myRequest.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage myResponse = await _httpClient.SendAsync(myRequest);

            if (myResponse.IsSuccessStatusCode)
            {
                var rawResult = myResponse.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

                Trace.TraceInformation($"Instance creation successfully queued: \n{result}");
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{myResponse.ReasonPhrase}'");
            }

            return result;
        }

        public async Task<OperationStatus> DeleteInstance(DeleteInstance deleteInstanceRequest)
        {
            OperationStatus result = null;

            this.ConnectToApi();

            string requestUrl = $"/api/v1/instances/{deleteInstanceRequest.InstanceId}/Delete";
            if (deleteInstanceRequest.IsValidateOnlyRequest)
            {
                requestUrl += "?validate=true";
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

                Trace.TraceInformation($"Successfully delete instance: \n{rawResult}");
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{response.ReasonPhrase}'");
            }

            return result;
        }

        public void Dispose()
        {
            if (_httpClient != null)
            { _httpClient.Dispose(); }
        }

        public async Task<IEnumerable<ServiceVersion>> GetServiceVersion(string name = "")
        {
            IEnumerable<ServiceVersion> result = null;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ServiceVersions");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;
                Trace.TraceInformation($"Successfully retrieve service version: \n{rawResult}");

                var res = this.ParseArray<ServiceVersion>(rawResult);

                if (!string.IsNullOrEmpty(name))
                {
                    result = res.Where(x => x.Name == name);
                }
                else
                {
                    result = res;
                }
            }
            else
            {
                Trace.TraceError($"Failed to retrieve service version '{response.ReasonPhrase}'");
            }

            return result;
        }

        public async Task<OperationStatus> GetOperationStatus(GetOperationStatus getOperationStatusRequest)
        {
            OperationStatus result = null;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/Operation/{getOperationStatusRequest.OperationId}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;
                Trace.TraceInformation($"Retrieving operation result: \n{rawResult}");

                result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{response.ReasonPhrase}'");
            }

            return result;
        }

        private IEnumerable<T> ParseArray<T>(string rawResult)
        {
            var r = JArray.Parse(rawResult);
            return r.ToObject<List<T>>();
        }

        private void ConnectToApi()
        {
            // Discover authority for the Online Management API service
            var authority = Authentication.DiscoverAuthority(_serviceUrl);

            // Authenticate to the Online Management API service by 
            // passing in the discovered authority 
            Authentication auth = new Authentication(authority.Result.ToString());

            // Use an HttpClient object to connect to Online Management API service.           
            _httpClient = new HttpClient(auth.ClientHandler, true);

            // Specify the API service base address and the max period of execution time 
            _httpClient.BaseAddress = new Uri(_serviceUrl);
            _httpClient.Timeout = new TimeSpan(0, 2, 0);
        }

    }
}