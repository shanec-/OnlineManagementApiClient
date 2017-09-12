﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace OnlineManagementApiClient.Service
{
    /// <summary>
    /// Manages authentication to the Online Management API service.
    /// Uses Microsoft Azure Active Directory Authentication Library (ADAL) 
    /// to handle the OAuth 2.0 protocol. 
    /// </summary>
    public class Authentication
    {
        private HttpMessageHandler _clientHandler = null;
        private AuthenticationContext _authContext = null;
        private string _authority = null;
        private string _serviceUrl = null;

        // Static variable to hold the Resource. This will be discovered
        // and then used later for acquiring access token
        private static string _resource = null;

        // TODO: Substitute your app registration values here.
        // These values are obtained on registering your application with the 
        // Azure Active Directory.
        private static string _clientId = "daeb7ab6-50dd-459d-acd8-acfe80226b9a";     //e.g. "e5cf0024-a66a-4fF6-85ce-99ba97a24bb2"
        private static string _redirectUrl = "http://localhost/25f91995-cbb3-448d-b0a1-300eb5ff47f0";  //e.g. "http://localhost/SdkSample"

        #region Constructors
        /// <summary>
        /// Base constructor.
        /// </summary>
        public Authentication() { }

        /// <summary>
        /// Custom constructor that allows adding an authority determined asynchronously before 
        /// instantiating the Authentication class.
        /// </summary>                
        /// <param name="authority">The URL of the authority.</param>
        public Authentication(string authority)
            : base()
        {
            Authority = authority;
            SetClientHandler();
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// The authentication context.
        /// </summary>
        public AuthenticationContext Context
        {
            get
            { return _authContext; }

            set
            { _authContext = value; }
        }

        /// <summary>
        /// The HTTP client message handler.
        /// </summary>
        public HttpMessageHandler ClientHandler
        {
            get
            { return _clientHandler; }

            set
            { _clientHandler = value; }
        }


        /// <summary>
        /// The URL of the authority to be used for authentication.
        /// </summary>
        public string Authority
        {
            get
            {
                if (_authority == null)
                {
                    var authority = DiscoverAuthority(_serviceUrl.ToString());
                    _authority = authority.Result.ToString();
                }


                return _authority;
            }

            set { _authority = value; }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Discover the authentication authority asynchronously.
        /// </summary>
        /// <param name="serviceUrl">The specified endpoint address</param>
        /// <returns>The URL of the authentication authority on the specified endpoint address, or an empty string
        /// if the authority cannot be discovered.</returns>
        public static async Task<string> DiscoverAuthority(string _serviceUrl)
        {
            try
            {
                AuthenticationParameters ap = await AuthenticationParameters.CreateFromResourceUrlAsync(
                    new Uri(_serviceUrl + "/api/aad/challenge"));
                _resource = ap.Resource;
                return ap.Authority;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("An HTTP request exception occurred during authority discovery.", e);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the authentication result for the configured authentication context.
        /// </summary>
        /// <returns>The refreshed access token.</returns>
        /// <remarks>Refresh the access token before every service call to avoid having to manage token expiration.</remarks>
        public AuthenticationResult AcquireToken()
        {
            //return _authContext.AcquireTokenAsync(_resource, _clientId, new Uri(_redirectUrl),
            //    PromptBehavior.Always).Result;

            //var platformParameters = new PlatformParameters(PromptBehavior.Always);

            var platformParameters = new PlatformParameters(PromptBehavior.Auto);
            return _authContext.AcquireTokenAsync(_resource, _clientId, new Uri(_redirectUrl), platformParameters).Result;
        }

        /// <summary>
        /// Sets the client message handler.
        /// </summary>
        private void SetClientHandler()
        {
#warning todo: make this a feature
#if DEBUG
            _clientHandler = new OAuthMessageHandler(this, new HttpClientHandler() { Proxy = new WebProxy("localhost", 8888)});
#else
            _clientHandler = new OAuthMessageHandler(this, new HttpClientHandler());
#endif
            _authContext = new AuthenticationContext(Authority, false);
        }

        #endregion Methods

        /// <summary>
        /// Custom HTTP client handler that adds the Authorization header to message requests.
        /// </summary>
        class OAuthMessageHandler : DelegatingHandler
        {
            Authentication _auth = null;

            public OAuthMessageHandler(Authentication auth, HttpMessageHandler innerHandler)
                : base(innerHandler)
            {
                _auth = auth;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                // It is a best practice to refresh the access token before every message request is sent. Doing so
                // avoids having to check the expiration date/time of the token. This operation is quick.
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.AcquireToken().AccessToken);
#if DEBUG
                System.Diagnostics.Debug.WriteLine(request.Headers.Authorization);
#endif
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}