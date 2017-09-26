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


namespace OnlineManagementApiClient.Service.Model
{
    /// <summary>
    /// Operation Status Response
    /// </summary>
    public class OperationStatusResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the operation status.
        /// </summary>
        /// <value>
        /// The operation status.
        /// </value>
        public OperationStatus OperationStatus { get; set; }


        /// <summary>
        /// Gets or sets the error reason.
        /// </summary>
        /// <value>
        /// The error reason.
        /// </value>
        public string ErrorReason { get; set; }
    }
}
