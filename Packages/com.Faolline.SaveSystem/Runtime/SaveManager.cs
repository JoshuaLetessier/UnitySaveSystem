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

        public Dictionary<Type, T> Load(string key)
        {
            Dictionary<Type, T> results = new();
            foreach (var system in saveSystems)
            {
                if (system.Value.Exists(key))
                {
                    results[system.Key] = system.Value.Load(key);
                }
            }
            return results;
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
    }
}
