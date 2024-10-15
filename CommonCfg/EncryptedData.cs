using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonCfg
{
    public class EncryptedData : IEncryptedData
    {
        private readonly AesCryptoServiceProviderClass _aesCryptoProvider;

        #region Constructor
        public EncryptedData()
        {
            _aesCryptoProvider = new AesCryptoServiceProviderClass();
        }
        #endregion

        #region Encode
        public virtual string Encode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(text);

            // encryption
            using (ICryptoTransform encrypt = _aesCryptoProvider.Aes().CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // Convert byte array to Base64 strings
                string s = Convert.ToBase64String(dest);
                return s;
            }
        }
        #endregion

        #region Decode
        public virtual string Decode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;


            // Convert Base64 strings to byte array
            byte[] src = System.Convert.FromBase64String(text);

            // decryption
            using (ICryptoTransform decrypt = _aesCryptoProvider.Aes().CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }
        #endregion
    }
}
