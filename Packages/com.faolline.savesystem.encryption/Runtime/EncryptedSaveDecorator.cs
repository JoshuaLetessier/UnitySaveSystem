namespace SaveSystem
{
    namespace SSEncrypted
    {

        public class EncryptedSaveDecorator<T> : ISaveSystem<T>
        {
            private readonly ISaveSystem<T> _baseSaveSystem;

            public EncryptedSaveDecorator(ISaveSystem<T> baseSaveSystem)
            {
                _baseSaveSystem = baseSaveSystem;
            }

            public void Save(string key, T data)
            {
                string json = JsonConvert.SerializeObject(data);
                string encryptedJson = EncryptionUtility.Encrypt(json);
                _baseSaveSystem.Save(key, (T)(object)encryptedJson);
            }

            public T Load(string key)
            {
                string encryptedJson = (string)(object)_baseSaveSystem.Load(key);
                if (string.IsNullOrEmpty(encryptedJson)) return default;

                string decryptedJson = EncryptionUtility.Decrypt(encryptedJson);
                return JsonConvert.DeserializeObject<T>(decryptedJson);
            }

            public void Delete(string key)
            {
                _baseSaveSystem.Delete(key);
            }

            public bool Exists(string key)
            {
                return _baseSaveSystem.Exists(key);
            }
        }
    }
}
