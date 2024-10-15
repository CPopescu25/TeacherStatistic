using CommonCfg;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Enums;

namespace TS.Data.Context.Models
{
    public class User
    {
        private readonly IEncryptedData _encryptedData;
        private string _password { get; set; }
        private string _email { get; set; }
        private string _phone { get; set; }

        #region Constructor
        public User()
        {
            _encryptedData = new EncryptedData();

            this.UserUniversity = new HashSet<UserUniversity>();
            this.UserGroup = new HashSet<UserGroup>();
        }
        #endregion

        #region Properties
        [Key]
        public Guid ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        //Encrypt and decrypt password
        [Column("Password")]
        public string EncryptedPassword
        {
            get { return _password; }
            set { _password = value; }
        }

        //Encrypt and decrypt email
        [Column("Email")]
        public string EncryptedEmail
        {
            get { return _email; }
            set { _email = value; }
        }

        //Encrypt and decrypt phone
        [Column("Phone")]
        public string EncryptedPhone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z0-9_.-]+$")]
        public String Password
        {
            get { return _encryptedData.Decode(EncryptedPassword); }
            set { EncryptedPassword = _encryptedData.Encode(value); }
        }

        [Required]
        [NotMapped]
        [DataType(DataType.EmailAddress)]
        public String Email
        {
            get { return _encryptedData.Decode(EncryptedEmail); }
            set { EncryptedEmail = _encryptedData.Encode(value); }
        }

        [Required]
        [NotMapped]
        [DataType(DataType.PhoneNumber)]
        public String Phone
        {
            get { return _encryptedData.Decode(EncryptedPhone); }
            set { EncryptedPhone = _encryptedData.Encode(value); }
        }

        public bool Enable { get; set; }

        public Byte[] Image { get; set; }

        public Year Year { get; set; }

        //University and department
        public virtual ICollection<UserUniversity> UserUniversity { get; set; }

        public virtual ICollection<UserGroup> UserGroup { get; set; }

        #endregion
    }
}
