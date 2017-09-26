using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineManagementApiClient.Service.Model
{
    public class RestoreInstanceBackup
    {
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        /// <remarks>
        /// The CreatedOn date and time of an existing instance backup to use.
        /// </remarks>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the instance backup identifier.
        /// </summary>
        /// <value>
        /// The instance backup identifier.
        /// </value>
        /// <remarks>
        /// The Id of a legacy instance backup to use. If set, this will be used instead of Label or RestorePoint.
        /// </remarks>
        public Guid InstanceBackupId { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        /// <remarks>
        /// The label of an existing instance backup to use. If set, this will be used instead of RestorePoint.
        /// </remarks>
        public string Label { get; set; }

        public Guid SourceInstanceId { get; set; }
    }
}
