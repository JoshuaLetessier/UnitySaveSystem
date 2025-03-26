using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager<T>
    {
        private Dictionary<Type, ISaveSystem<T>> saveSystems = new();

        public void Register<TSystem>(TSystem system) where TSystem : ISaveSystem<T>
        {
            saveSystems[typeof(TSystem)] = system;
        }


        public void Save(string key, T data)
        {
            foreach (var system in saveSystems.Values)
            {
                system.Save(key, data);
            }
        }

        public LoadResult<T> Load(string key)
        {

            var results  = new Dictionary<Type, T>();

            foreach(var system in saveSystems.Values)
            {
                if (system.Exists(key))
                {
                    var data = system.Load(key);
                    if(data != null)
                    {
                        results[system.GetType()] = data;
                    }
                }
            }

            return new LoadResult<T>(results);
        }

        public T LoadFrom<TSystem>(string key) where TSystem : ISaveSystem<T>
        {
            if (saveSystems.TryGetValue(typeof(TSystem), out var system))
            {
                return system.Load(key);
            }
            return default;
        }

        public void Delete(string key)
        {
            foreach (var system in saveSystems.Values)
            {
                system.Delete(key);
            }
        }

        public bool Exists(string key)
        {
            foreach (var system in saveSystems.Values)
            {
                if (system.Exists(key))
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<Type, ISaveSystem<T>> GetSaveSystems()
        {
            return saveSystems;
        }
    }
}
