using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibTimeCreator
{
    public class LibTimeInfo:IValidatableObject
    {
        public LibTimeInfo()
        {

        }
        public LibTimeInfo(int afterMinutes)
        {
            if (afterMinutes <= 0)
                throw new ArgumentException("cannot be in the past");
            this.MaxDate = SystemTime.Now().AddMinutes(afterMinutes);

        }
        public string Info { get; set; }
        public DateTimeOffset MaxDate {get;set;}
        static string formatDate = "yyyyMMddHHmmss";
        public string Generate()
        {
            return Encrypt(MaxDate.ToString(formatDate) + Info);
        }
        public static LibTimeInfo FromString(string s)
        {
            //TODO: make error proof
            var dec = Decrypt(s);
            var date = DateTimeOffset.ParseExact(dec.Substring(0, formatDate.Length), formatDate,null);
            var ret = new LibTimeInfo();
            ret.MaxDate = date;
            ret.Info = dec.Substring(formatDate.Length);
            return ret;

        }
        #region dirty fast encrypt / decrypt
        static string EncryptionKey = "Andrei";
        static string Decrypt(string cipherText)
        {
            byte[] IV = Convert.FromBase64String(cipherText.Substring(0, 20));
            cipherText = cipherText.Substring(20).Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] IV = new byte[15];
                var r = new Random();
                r.NextBytes(IV);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(IV) + Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        #endregion
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var now = SystemTime.Now();
            if (MaxDate< now)
                yield return new ValidationResult("date not good");
        }
        public bool IsValid()
        {
            return !Validate(null).Any();
        }
    }
}
