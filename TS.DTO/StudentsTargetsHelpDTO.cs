using CommonCfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class StudentsTargetsHelpDTO
    {
        private readonly IEncryptedData _encryptedData;
        private string _email { get; set; }
        private string _Password { get; set; }

        public StudentsTargetsHelpDTO()
        {
            _encryptedData = new EncryptedData();
        }

        public Guid Id { get; set; }

        public string User_Name { get; set; }

        public string EncryptedValue
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Email
        {
            get { return _encryptedData.Decode(EncryptedValue); }
            set { EncryptedValue = _encryptedData.Encode(value); }
        }

        //Encrypt or decrypt password

        public string EncryptedPassword
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public String Password
        {
            get { return _encryptedData.Decode(EncryptedPassword); }
            set { EncryptedPassword = _encryptedData.Encode(value); }
        }

        public string Message { get; set; }
    }
}
