using CommonCfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class ChangePasswordDTO
    {
        private readonly IEncryptedData _encryptedData;
        private string _oldPassword { get; set; }
        private string _newPassword{ get; set; }

        #region Constructor
        public ChangePasswordDTO()
        {
            _encryptedData = new EncryptedData();

       }
        #endregion

        #region Properties
        public string Username { get; set; }

        //Encrypt and decrypt old password
        public string EncryptedOldPassword
        {
            get { return _oldPassword; }
            set { _oldPassword = value; }
        }

        //Encrypt and decrypt new password
        public string EncryptedNewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; }
        }

        public String OldPassword
        {
            get { return _encryptedData.Decode(EncryptedOldPassword); }
            set { EncryptedOldPassword = _encryptedData.Encode(value); }
        }

        public String NewPassword
        {
            get { return _encryptedData.Decode(EncryptedNewPassword); }
            set { EncryptedNewPassword = _encryptedData.Encode(value); }
        }

        public string Email { get; set; }
        #endregion
    }
}
