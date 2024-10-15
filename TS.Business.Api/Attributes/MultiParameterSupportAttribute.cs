using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Business.Api.Attributes
{
    /// <summary>
    /// Mark a web api POST action as using multiple parameters in body
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MultiParameterSupportAttribute : Attribute
    {

    }
}