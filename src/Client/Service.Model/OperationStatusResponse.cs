using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
