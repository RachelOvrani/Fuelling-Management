using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Manager
{


    public class EncrpytionHelper
    {

        //private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourFixedKey1234");
        //private static readonly byte[] IV = Encoding.UTF8.GetBytes("YourFixedIV67890");


        //public static string EncryptString(string plainText)
        //{
        //    byte[] encryptedBytes;

        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Key;
        //        byte[] iv = IV;

        //        using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            msEncrypt.Write(iv, 0, iv.Length);
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //            {
        //                swEncrypt.Write(plainText);
        //            }

        //            encryptedBytes = msEncrypt.ToArray();
        //        }
        //    }

        //    string encryptedString = Convert.ToBase64String(encryptedBytes);
        //    return encryptedString;
        //}

        //public static string DecryptString(string encryptedString)
        //{
        //    byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
        //    string plainText;

        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Key;
        //        byte[] iv = IV;
        //        Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);

        //        using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv))
        //        using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
        //        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //        {
        //            plainText = srDecrypt.ReadToEnd();
        //        }
        //    }

        //    return plainText;
        //}


        private static readonly string EncryptionKey = "RachelOvrani1234";

        public static string EncryptString(string plainText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    plainText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return plainText;
        }

        public static string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
                {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

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



        public static readonly int EncodingKey = 20;
        // הצפנת סיסמה
        public static string EncodingPassword(string password)
        {
            string encodingPassword = "";
            for (int i = 0; i < password.Length; i++)
            {
                encodingPassword += (Char)(password[i] + EncodingKey);
            }

            return encodingPassword;
        }

        // פיענוח סיסמה
        public static string DecodingPassword(string password)
        {
            string decodingPassword = "";
            for (int i = 0; i < password.Length; i++)
            {
                decodingPassword += (Char)(password[i] - EncodingKey);
            }

            return decodingPassword;
        }

    }
}

