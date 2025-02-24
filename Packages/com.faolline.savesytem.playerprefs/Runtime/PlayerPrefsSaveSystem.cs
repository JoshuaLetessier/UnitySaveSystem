using UnityEngine;
using Newtonsoft.Json;

namespace SaveSystem
{
    public class PlayerPrefsSaveSystem<T> : ISaveSystem<T>
    {
        public void Save(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"[PlayerPrefs] Sauvegarde : {key} = {json}");
        }

        public T Load(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogError($"[PlayerPrefs] Aucune donnée trouvée pour la clé '{key}'");
                return default;
            }

            string json = PlayerPrefs.GetString(key);
            Debug.Log($"[PlayerPrefs] Chargé : {json}");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"[PlayerPrefs] Clé supprimée : {key}");
        }

        public bool Exists(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}
