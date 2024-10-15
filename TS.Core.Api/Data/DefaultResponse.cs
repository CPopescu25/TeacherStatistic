using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api.Data
{
    /// <summary>
    /// Default response instance to handle the error cases
    /// </summary>
    public class DefaultResponse : IDefaultResponse
    {
        #region Implementation of IDefaultResponse

        /// <summary>
        /// HTML Status code for the response
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The reason in case of error
        /// </summary>
        public string DeveloperMessage { get; set; }

        /// <summary>
        /// The friendly error message
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// API specific error code
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Additional info on error status
        /// </summary>
        public string MoreInfo { get; set; }

        /// <summary>
        /// Success marker for the request
        /// </summary>
        public bool Success { get; set; }

        #endregion
    }
}
