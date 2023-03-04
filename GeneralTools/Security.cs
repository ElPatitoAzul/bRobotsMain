using System;
using System.Security.Cryptography;
using System.Text;

namespace GeneralTools
{
    public class Security
    {
        private static string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
    //private const string passPhrase = "TdmAAI%2022";
    //private const string initVector = "pemgai88luzxpgzl";
    //private const int keysize = 256;
    //public string Encrypt(string plainText)
    //{
    //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
    //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
    //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
    //    byte[] keyBytes = password.GetBytes(keysize / 8);
    //    var symmetricKey = new RijndaelManaged();
    //    symmetricKey.Mode = CipherMode.CBC;
    //    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, keyBytes);
    //    MemoryStream memoryStream = new MemoryStream();
    //    CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    //    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
    //    cryptoStream.FlushFinalBlock();
    //    byte[] cipherTextBytes = memoryStream.ToArray();
    //    memoryStream.Close();
    //    cryptoStream.Close();
    //    return Convert.ToBase64String(cipherTextBytes);
    //}

    //public string Decrypt(string cipherText)
    //{
    //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
    //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
    //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
    //    byte[] keyBytes = password.GetBytes(keysize / 8);
    //    var symmetricKey = new RijndaelManaged();
    //    symmetricKey.Mode = CipherMode.CBC;
    //    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, keyBytes);
    //    MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
    //    CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
    //    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
    //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
    //    memoryStream.Close();
    //    cryptoStream.Close();
    //    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    //}




}