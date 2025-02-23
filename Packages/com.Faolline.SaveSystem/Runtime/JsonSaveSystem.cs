using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SaveSystem
{
    public class JsonSaveSystem<T> : ISaveSystem<T>
    {
        private readonly string saveDirectory = Application.persistentDataPath + "/Saves/";

        public JsonSaveSystem()
        {
            if (!Directory.Exists(saveDirectory))
            { Directory.CreateDirectory(saveDirectory); 
                Debug.Log($"Dossier de sauvegarde créé : {saveDirectory}");
            }
        }

        public void Save(string key, T data)
        {
            try
            {
                string path = GetPath(key);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                string checksum = ComputeHash(json);

                // 🔹 On sépare proprement les données du checksum
                string fullContent = json + "\n---CHECKSUM---\n" + checksum;

                File.WriteAllText(path, fullContent);
                Debug.Log($"Fichier sauvegardé : {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"ERREUR lors de la sauvegarde : {e.Message}");
            }
        }


        public T Load(string key)
        {
            string path = GetPath(key);
            if (!File.Exists(path))
            {
                Debug.LogError($"Fichier introuvable : {path}");
                return default;
            }

            try
            {
                // 🔹 Lire tout le fichier
                string fullContent = File.ReadAllText(path);

                // 🔹 Séparer JSON et Checksum
                string[] parts = fullContent.Split(new string[] { "\n---CHECKSUM---\n" }, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    Debug.LogError("Fichier corrompu ou format invalide !");
                    return default;
                }

                string json = parts[0];
                string savedChecksum = parts[1];

                // 🔹 Vérifier l'intégrité des données
                if (ComputeHash(json) != savedChecksum)
                {
                    Debug.LogError("Données corrompues !");
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"ERREUR lors du chargement du fichier : {e.Message}");
                return default;
            }
        }


        public void Delete(string key)
        {
            string path = GetPath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Fichier supprimé : {path}");
            }
        }

        public bool Exists(string key)
        {
            return File.Exists(GetPath(key));
        }

        private string GetPath(string key)
        {
            return Path.Combine(saveDirectory, $"{key}.json");
        }

        private string ComputeHash(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
