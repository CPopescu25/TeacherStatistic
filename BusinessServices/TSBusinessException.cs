﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    /// <summary>
    /// Represents errors that occur during application execution
    /// </summary>
    [Serializable]
    public class TSBusinessException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the Exception class.
        /// </summary>
        public TSBusinessException() : this("")
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TSBusinessException(string message)
            : this(message, "", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public TSBusinessException(string messageFormat, params object[] args)
            : this(string.Format(messageFormat, args), "", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected TSBusinessException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="code">Exception code</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TSBusinessException(string message, string code, Exception innerException)
            : this(message, code, innerException, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="code">Exception code</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="args">The exception message arguments.</param>
        public TSBusinessException(string message, string code, Exception innerException, params object[] args)
            : base(message, innerException)
        {
        }

    }
}
