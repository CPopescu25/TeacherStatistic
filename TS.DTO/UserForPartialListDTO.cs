using CommonCfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class UserForPartialListDTO
    {
        private readonly IEncryptedData _encryptedData;
        private string _Email { get; set; }

        public UserForPartialListDTO()
        {
            _encryptedData = new EncryptedData();
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string EncryptedEmail
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public String Email
        {
            get { return _encryptedData.Decode(EncryptedEmail); }
            set { EncryptedEmail = _encryptedData.Encode(value); }
        }
    }
}
