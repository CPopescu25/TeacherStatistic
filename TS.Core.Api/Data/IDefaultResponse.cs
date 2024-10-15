using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api.Data
{
    /// <summary>
    /// The default response message
    /// </summary>
    public interface IDefaultResponse
    {
        /// <summary>
        /// HTML Status code for the response
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// The reason in case of error
        /// </summary>
        string DeveloperMessage { get; set; }

        /// <summary>
        /// The friendly error message
        /// </summary>
        string UserMessage { get; set; }

        /// <summary>
        /// API specific error code
        /// </summary>
        string ErrorCode { get; set; }

        /// <summary>
        /// Additional info on error status
        /// </summary>
        string MoreInfo { get; set; }

        /// <summary>
        /// Success marker for the request
        /// </summary>
        bool Success { get; set; }

    }
}
