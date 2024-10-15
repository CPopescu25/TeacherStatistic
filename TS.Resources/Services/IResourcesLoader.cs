using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Resources.Services
{
    public interface IResourcesLoader
    {
        string GetStringResourceByName(string resourceName);

        string GetStringResourceByName(string resourceName, CultureInfo currentCulture);
    }
}
