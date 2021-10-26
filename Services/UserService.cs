using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Rotativa;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Services
{
    public class UserService
    {
        private static readonly uhrenWeltEntities db = new uhrenWeltEntities();

        public static string CreateSalt(int saltLength)
        {
            var s = "";
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            using (var provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != saltLength)
                {
                    var oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    var character = (char)oneByte[0];
                    if (validChars.Contains(character)) s += character;
                }
            }
            return s;
        }

        public static string HashPassword(string password)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) result.Append(hash[i].ToString("X2"));
            return result.ToString();
        }

        public static bool LoginCheck(string email, string password)
        {
            var findEmail = db.Customer.Where(x => x.Email == email);

            if (findEmail.Count() > 0)
            {
                var salt = db.Customer.Where(x => x.Email == email).Select(s => s.Salt).Single();
                var hashPassword = HashPassword(password + salt);
                var checkPasswordHash = db.Customer.Where(x => x.PwHash == hashPassword);

                if (checkPasswordHash.Count() > 0) return true;
                return false;
            }
            return false;
        }

        public static bool EmailCheck(string email)
        {
            var checkEmail = db.Customer.Where(x => x.Email == email);
            if (checkEmail.Count() > 0) return true;
            return false;
        }

        public static bool SendEmail(OrderVM oderVm, string customerEmail, byte[] invoicePdf)
        {
            var message = new MailMessage(@"testmailuhrenwelt@gmail.com", customerEmail);
            message.Subject = "Vielen Danke für Ihre Bestellung bei uhrenwelt.at!";
            message.Body = "Vielen Dank!\n Ihre Rechnung.";
            //message.Attachments
            SmtpClient mailer = new SmtpClient("smtp.office365.com", 587);
            mailer.Credentials = new NetworkCredential("testmailuhrenwelt@gmail.com", "User123!");
            mailer.EnableSsl = true;
            mailer.Send(message);
            return true;
        }


        public static int GetUserIdByEmail(string email)
        {
            var getUser = db.Customer.Single(x => x.Email == email);

            if (getUser != null)
                return getUser.Id;

            return -1;
        }
    }
}