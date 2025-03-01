using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class EncryptionUtility
{
    private static readonly string KeySavePath = Application.persistentDataPath + "/encryption_key.dat";

    private static byte[] encryptionKey;

    static EncryptionUtility()
    {
        LoadOrGenerateKey();
    }

    private static void LoadOrGenerateKey()
    {
        if (File.Exists(KeySavePath))
        {
            encryptionKey = Convert.FromBase64String(File.ReadAllText(KeySavePath));
        }
        else
        {
            encryptionKey = GenerateSecureKey();
            File.WriteAllText(KeySavePath, Convert.ToBase64String(encryptionKey));
        }
    }

    private static byte[] GenerateSecureKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[32]; // 256-bit key
            rng.GetBytes(key);
            return key;
        }
    }

    public static string Encrypt(string plainText)
    {
        byte[] iv = new byte[16]; // IV de 16 bytes
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv);
        }

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = encryptionKey;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(iv, 0, iv.Length); // Stocker l'IV au début

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        byte[] iv = new byte[16];
        Array.Copy(cipherBytes, 0, iv, 0, iv.Length);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = encryptionKey;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
