using Manager.FuelingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public static class Global
    {
        public static FuelingManagmentServiceClient Client = new FuelingManagmentServiceClient();

        public static string DefaultColorHex = "#FF00BCD4";



        public static readonly int EncodingKey = 20;


        // הצפנת סיסמה
        public static string EncodingPassword(string password)
        {
            string encodingPassword = "";
            for (int i = 0; i < password.Length; i++)
            {
                encodingPassword += (char)(password[i] + EncodingKey);
            }

            return encodingPassword;
        }

        // פיענוח סיסמה
        public static string DecodingPassword(string password)
        {
            string decodingPassword = "";
            for (int i = 0; i < password.Length; i++)
            {
                decodingPassword += (char)(password[i] - EncodingKey);
            }

            return decodingPassword;
        }
    }
}
