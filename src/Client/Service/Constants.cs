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

namespace OnlineManagementApiClient.Service
{
    /// <summary>
    /// Constants
    /// </summary>
    public sealed class Constants
    {
        /// <summary>
        /// Constants related to services.
        /// </summary>
        public sealed class Services
        {
            /// <summary>
            /// Available instance types
            /// </summary>
            public sealed class InstanceType
            {
                /// <summary>
                /// The production instance id
                /// </summary>
                public const int Production = 1;

                /// <summary>
                /// The sandbox instance id 
                /// </summary>
                public const int Sandbox = 2;
            }

            /// <summary>
            /// Available languages
            /// </summary>
            public sealed class Languages
            {
                /// <summary>
                /// The english
                /// </summary>
                public const string English = "1033";
            }
        }

        /// <summary>
        /// Constants for the configuration keys
        /// </summary>
        public sealed class ConfigurationKeys
        {
            /// <summary>
            /// Authentication configuration keys
            /// </summary>
            public sealed class Authentication
            {
                /// <summary>
                /// The key prefix
                /// </summary>
                private const string Prefix = "app:auth:";

                /// <summary>
                /// The client identifier
                /// </summary>
                public const string ClientId = Prefix + "clientId";

                /// <summary>
                /// The redirect URL
                /// </summary>
                public const string RedirectUrl = Prefix + "redirectUrl";
            }

            /// <summary>
            /// Web proxy configuration keys
            /// </summary>
            public sealed class WebProxy
            {
                /// <summary>
                /// The key prefix
                /// </summary>
                private const string Prefix = "app:web-proxy:";
                /// <summary>
                /// The web proxy enabled
                /// </summary>
                public const string Enabled = Prefix + "True";
                
                /// <summary>
                /// The web proxy server name
                /// </summary>
                public const string ServerName = Prefix + "localhost";
                
                /// <summary>
                /// The web proxy port
                /// </summary>
                public const string Port = Prefix + "8888";
            }
        }
    }
}
