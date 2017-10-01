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
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineManagementApiClient.Service.Model;

namespace OnlineManagementApiClient.Service
{
    /// <summary>
    /// Customer Engagement Online Management REST service implementation.
    /// </summary>
    /// <seealso cref="OnlineManagementApiClient.Service.IOnlineManagementAgent" />
    /// <seealso cref="System.IDisposable" />
    public class CrmOnlineManagmentRestService : IOnlineManagementAgent, IDisposable
    {
        private HttpClient _httpClient;
        private string _serviceUrl;
        private string _username;
        private string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmOnlineManagmentRestService" /> class.
        /// </summary>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public CrmOnlineManagmentRestService(string serviceUrl, string username, string password)
        {
            this._serviceUrl = serviceUrl;
            this._username = username;
            this._password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmOnlineManagmentRestService"/> class.
        /// </summary>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public CrmOnlineManagmentRestService(string serviceUrl, HttpClient httpClient)
        {
            this._serviceUrl = serviceUrl;
            this._httpClient = httpClient;
        }

        /// <summary>
        /// Retrieve a Customer Engagement instance in your Office 365 tenant.
        /// </summary>
        /// <returns>
        /// Enumerable list of available instances.
        /// </returns>
        public async Task<IEnumerable<Instance>> GetInstances()
        {
            IEnumerable<Instance> result = null;

            this.ConnectToApi();

            var myRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/instances");
            var myResponse = await _httpClient.SendAsync(myRequest);

            if (myResponse.IsSuccessStatusCode)
            {
                var rawResult = myResponse.Content.ReadAsStringAsync().Result;

                Trace.TraceInformation($"Your instances retrieved from Office 365 tenant: \n{rawResult}");

                result = this.ParseArray<Instance>(rawResult);
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

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<OperationStatusResponse> CreateInstance(CreateInstance request)
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

            HttpResponseMessage response = await _httpClient.SendAsync(myRequest);
            var rawResult = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                 Trace.TraceInformation($"Instance creation successfully queued: \n{result}");
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{response.ReasonPhrase}'");
            }

            result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

            return new OperationStatusResponse() { IsSuccess = response.IsSuccessStatusCode, OperationStatus = result };
        }

        /// <summary>
        /// Deletes a Customer Engagement instance in your Office 365 tenant.
        /// </summary>
        /// <param name="deleteInstanceRequest">The delete instance request.</param>
        /// <returns>
        /// Operation result.
        /// </returns>
        public async Task<OperationStatusResponse> DeleteInstance(DeleteInstance deleteInstanceRequest)
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

            var rawResult = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                Trace.TraceInformation($"Successfully delete instance: \n{rawResult}");
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{response.ReasonPhrase}'");
            }

            result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

            return new OperationStatusResponse() { IsSuccess = response.IsSuccessStatusCode, OperationStatus = result };
        }

        public void Dispose()
        {
            if (_httpClient != null)
            { _httpClient.Dispose(); }
        }

        /// <summary>
        /// Retrieve information about all the supported releases for Customer Engagement.
        /// </summary>
        /// <returns>
        /// Enumerable list of service versions.
        /// </returns>
        public async Task<IEnumerable<ServiceVersion>> GetServiceVersion()
        {
            IEnumerable<ServiceVersion> result = null;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ServiceVersions");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;
                Trace.TraceInformation($"Successfully retrieve service version: \n{rawResult}");

                result = this.ParseArray<ServiceVersion>(rawResult);
            }
            else
            {
                Trace.TraceError($"Failed to retrieve service version '{response.ReasonPhrase}'");
            }

            return result;
        }

        /// <summary>
        /// Retrieves status of an operation in your Customer Engagement instance.
        /// </summary>
        /// <param name="getOperationStatusRequest">The get operation status request.</param>
        /// <returns>
        /// Operation result.
        /// </returns>
        public async Task<OperationStatusResponse> GetOperationStatus(GetOperationStatusRequest getOperationStatusRequest)
        {
            OperationStatus result = null;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/Operation/{getOperationStatusRequest.OperationId}");
            var response = await _httpClient.SendAsync(request);

            var rawResult = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                Trace.TraceInformation($"Retrieving operation result: \n{rawResult}");
            }
            else
            {
                Trace.TraceError($"The request failed with a status of '{response.ReasonPhrase}'");
            }

            result = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

            return new OperationStatusResponse() { IsSuccess = response.IsSuccessStatusCode, OperationStatus = result };
        }

        /// <summary>
        /// Parses a string into an JArray of a specified type.
        /// </summary>
        /// <typeparam name="T">Specified type.</typeparam>
        /// <param name="content">The raw result.</param>
        /// <returns>Enumerable type of T</returns>
        private IEnumerable<T> ParseArray<T>(string content)
        {
            var r = JArray.Parse(content);
            return r.ToObject<List<T>>();
        }

        /// <summary>
        /// Connects to API.
        /// </summary>
        private void ConnectToApi()
        {
            // Discover authority for the Online Management API service
            var authority = Authentication.DiscoverAuthority(_serviceUrl);

            // Authenticate to the Online Management API service by 
            // passing in the discovered authority 
            Authentication auth = new Authentication(authority.Result.ToString(), this._username, this._password);

            // Use an HttpClient object to connect to Online Management API service.           
            _httpClient = new HttpClient(auth.ClientHandler, true);

            // Specify the API service base address and the max period of execution time 
            _httpClient.BaseAddress = new Uri(_serviceUrl);
            _httpClient.Timeout = new TimeSpan(0, 2, 0);
        }

    }
}
