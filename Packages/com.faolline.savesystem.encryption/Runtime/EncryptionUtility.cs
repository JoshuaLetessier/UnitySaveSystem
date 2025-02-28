using UnityEngine;
using Newtonsoft.Json;

public class EncryptedPlayerPrefsSaveSystem<T> : ISaveSystem<T>
{
    public void Save(string key, T data)
    {
        string json = JsonConvert.SerializeObject(data);
        string encryptedJson = EncryptionUtility.Encrypt(json); // 🔒 Encrypt data
        PlayerPrefs.SetString(key, encryptedJson);
        PlayerPrefs.Save();
    }

    public T Load(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return default;

        string encryptedJson = PlayerPrefs.GetString(key);
        string decryptedJson = EncryptionUtility.Decrypt(encryptedJson); // 🔓 Decrypt data
        return JsonConvert.DeserializeObject<T>(decryptedJson);
    }

    public void Delete(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
