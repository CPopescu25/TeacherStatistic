using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CommonCfg
{
    public class AesCryptoServiceProviderClass
    {
        public AesCryptoServiceProvider Aes()
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            string AesIV256 = ConfigurationManager.AppSettings["AesIV"];
            string AesKey256 = ConfigurationManager.AppSettings["AesKey"];

            aes.IV = Encoding.UTF8.GetBytes(AesIV256);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }
    }
}
