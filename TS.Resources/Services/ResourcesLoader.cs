using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TS.Resources.Services
{
    public class ResourcesLoader : IResourcesLoader
    {
        #region Fields

        #endregion

        #region Constructor

        public ResourcesLoader()
        {

        }

        #endregion

        #region Implementation of IResourcesLoader

        public string GetStringResourceByName(string resourceName)
        {
            return GetStringResourceByName(resourceName, Thread.CurrentThread.CurrentUICulture);
        }

        public string GetStringResourceByName(string resourceName, CultureInfo currentCulture)
        {
            try
            {
                return Properties.Resources.ResourceManager.GetString(resourceName, currentCulture);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion
    }
}
