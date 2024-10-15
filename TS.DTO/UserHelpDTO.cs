using CommonCfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Enums;
using TS.Data.Context.Models;

namespace TS.DTO
{
    public class UserHelpDTO
    {
        private readonly IEncryptedData _encryptedData;
        private string _Password { get; set; }
        private string _Email { get; set; }
        private string _Phone { get; set; }

        public UserHelpDTO()
        {
            _encryptedData = new EncryptedData();
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

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
        public string EncryptedPhone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public String Phone
        {
            get { return _encryptedData.Decode(EncryptedPhone); }
            set { EncryptedPhone = _encryptedData.Encode(value); }
        }

        public Byte[] Image { get; set; }

        public bool Enable { get; set; }

        //for index
        public List<JsTreeHelpDTO> Universities { get; set; }
        public List<JsTreeHelpDTO> Group { get; set; }

        //for details and edit
        public Dictionary<int, string> universities { get; set; }
        public Dictionary<string, string> groups { get; set; }

        public Year Year { get; set; }

        public virtual List<UserGroup> UserGroup { get; set; }
    }
}
