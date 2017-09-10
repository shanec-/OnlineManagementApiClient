using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineManagementApiClient.Service.Model;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OnlineManagementApiClient.Service
{
    public class CrmOnlineManagmentService : IOnlineManagementAgent, IDisposable
    {
        private HttpClient _httpClient;
        private string _serviceUrl;

        public CrmOnlineManagmentService(string serviceUrl)
        {
            this._serviceUrl = serviceUrl;
        }

        public CrmOnlineManagmentService(string serviceUrl, HttpClient httpClient)
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

                Console.WriteLine("Your instances retrieved from Office 365 tenant: \n{0}", rawResult);


                var r = JArray.Parse(rawResult);
                var res = ParseArray<Instance>(r);

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
                Console.WriteLine("The request failed with a status of '{0}'",
                       myResponse.ReasonPhrase);
            }

            return result;
        }

        public async Task<IEnumerable<OperationStatus>> CreateInstance(CreateInstanceRequest request)
        {
            this.ConnectToApi();

            string requestUrl = "/api/v1/instances/Provision/";
            if (request.IsValidateOnlyRequest)
            {
                requestUrl += "?validate";
            }

            HttpRequestMessage myRequest = new HttpRequestMessage(HttpMethod.Put, requestUrl);

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            myRequest.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage myResponse = await _httpClient.SendAsync(myRequest);

            if (myResponse.IsSuccessStatusCode)
            {
                var result = myResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Your instances retrieved from Office 365 tenant: \n{0}", result);
            }
            else
            {
                Console.WriteLine("The request failed with a status of '{0}'",
                       myResponse.ReasonPhrase);
            }

#warning todo: fix!
            return new List<OperationStatus>();
        }

        public async Task<OperationStatus> DeleteInstance(DeleteInstanceRequest deleteInstanceRequest)
        {
            OperationStatus result = null;

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

                var r = JsonConvert.DeserializeObject<OperationStatus>(rawResult);

                Console.WriteLine("Your instances retrieved from Office 365 tenant: \n{0}", rawResult);
            }
            else
            {
                Console.WriteLine("The request failed with a status of '{0}'",
                       response.ReasonPhrase);
            }

            return result;
        }

        public void Dispose()
        {
            if (_httpClient != null)
            { _httpClient.Dispose(); }
        }

        public async Task<Guid> GetServiceVersion(string name = "")
        {
            Guid result = Guid.Empty;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ServiceVersions");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Successfully retrieve service version: \n{0}", rawResult);

                var res = this.ParseArray<ServiceVersion>(rawResult);

                if (!string.IsNullOrEmpty(name))
                {
                    result = res.Where(x => x.Name == name)
                        .FirstOrDefault().Id;
                }
                else
                {
                    result = res.FirstOrDefault().Id;
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve service version '{0}'",
                       response.ReasonPhrase);
            }

            return result;
        }


        public async Task<IEnumerable<OperationStatus>> GetOperationStatus(GetOperationStatusRequest getOperationStatusRequest)
        {
            IEnumerable<OperationStatus> result = null;

            this.ConnectToApi();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/Operation/{getOperationStatusRequest.OperationId}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var rawResult = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Retrieving operation result: \n{0}", rawResult);
                result = this.ParseArray<OperationStatus>(rawResult);
            }
            else
            {
                Console.WriteLine("The request failed with a status of '{0}'",
                       response.ReasonPhrase);
            }

            return result;
        }

        private IEnumerable<T> ParseArray<T>(JArray array)
        {
            return array.ToObject<List<T>>();
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
