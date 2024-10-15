using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCfg
{
    /// <summary>
    /// top section definition class
    /// </summary>
    public class CustomAppConfigSection : ConfigurationSection
    {
        /// <summary>
        /// the definition of the child section collection of key value pairs
        /// </summary>
        [ConfigurationProperty("ECSection")]
        [ConfigurationCollection(typeof(CustomConfigCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public CustomConfigCollection CustCfg
        {
            get { return ((CustomConfigCollection)(base["ECSection"])); }
        }
    }

    /// <summary>
    /// defines the collection access methods
    /// </summary>
    [ConfigurationCollection(typeof(CustCfgElem))]
    public class CustomConfigCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Access collection elements by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CustCfgElem this[int index]
        {
            get { return (CustCfgElem)BaseGet(index); }
        }

        /// <summary>
        /// Access collection elements by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CustCfgElem this[object key]
        {
            get { return (CustCfgElem)BaseGet(key); }
        }

        /// <summary>
        /// Get key for element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustCfgElem)(element)).key;
        }

        /// <summary>
        /// Add new element to collection
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustCfgElem();
        }
    }

    /// <summary>
    /// defines the collection elements (key-value pairs)
    /// </summary>
    public class CustCfgElem : ConfigurationElement
    {
        /// <summary>
        /// class contructor
        /// </summary>
        public CustCfgElem() { }

        /// <summary>
        /// key definition
        /// </summary>
        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// value definition
        /// </summary>
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
