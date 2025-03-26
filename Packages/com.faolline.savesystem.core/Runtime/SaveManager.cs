using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Manages multiple save systems for a given data type T.
    /// Allows saving, loading, checking and deleting data using registered systems.
    /// </summary>
    /// <typeparam name="T">The type of data to save and load.</typeparam>
    public class SaveManager<T>
    {
        private readonly Dictionary<Type, ISaveSystem<T>> saveSystems = new();

        // -----------------------------
        // PART 1: Register
        // -----------------------------

        /// <summary>
        /// Registers a new save system.
        /// </summary>
        /// <typeparam name="TSystem">The type of the save system.</typeparam>
        /// <param name="system">The instance of the save system to register.</param>
        public void Register<TSystem>(TSystem system) where TSystem : ISaveSystem<T>
        {
            saveSystems[typeof(TSystem)] = system;
        }

        /// <summary>
        /// Unregisters a save system.
        /// </summary>
        /// <typeparam name="TSystem">The type of the save system to remove.</typeparam>
        public void Unregister<TSystem>() where TSystem : ISaveSystem<T>
        {
            saveSystems.Remove(typeof(TSystem));
        }

        // -----------------------------
        // PART 2: Save and Load
        // -----------------------------

        /// <summary>
        /// Saves data with the specified key using all registered systems.
        /// </summary>
        /// <param name="key">The key associated with the data.</param>
        /// <param name="data">The data to save.</param>
        public void Save(string key, T data)
        {
            foreach (var system in saveSystems.Values)
            {
                system.Save(key, data);
            }
        }

        /// <summary>
        /// Loads data from all systems where the key exists.
        /// </summary>
        /// <param name="key">The key associated with the data.</param>
        /// <returns>A LoadResult containing data from systems that successfully loaded it.</returns>
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

        /// <summary>
        /// Loads data from a specific save system.
        /// </summary>
        /// <typeparam name="TSystem">The save system type to load from.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <returns>The loaded data or default if not found.</returns>
        public T LoadFrom<TSystem>(string key) where TSystem : ISaveSystem<T>
        {
            if (saveSystems.TryGetValue(typeof(TSystem), out var system))
            {
                return system.Load(key);
            }
            return default;
        }

        /// <summary>
        /// Tries to load data from a specific save system.
        /// </summary>
        /// <typeparam name="TSystem">The save system type to load from.</typeparam>
        /// <param name="key">The key associated with the data.</param>
        /// <param name="data">The output loaded data.</param>
        /// <returns>True if the data exists and is loaded successfully; false otherwise.</returns>
        public bool TryLoadFrom<TSystem>(string key, out T data) where TSystem : ISaveSystem<T>
        {
            data = default;
            if (saveSystems.TryGetValue(typeof(TSystem), out var system) && system.Exists(key))
            {
                data = system.Load(key);
                return true;
            }
            return false;
        }

        // -----------------------------
        // PART 3: Check
        // -----------------------------

        /// <summary>
        /// Checks if a save system is registered.
        /// </summary>
        /// <typeparam name="TSystem">The type of the save system.</typeparam>
        /// <returns>True if registered; false otherwise.</returns>
        public bool IsSystemRegistered<TSystem>() where TSystem : ISaveSystem<T>
        {
            return saveSystems.ContainsKey(typeof(TSystem));
        }

        /// <summary>
        /// Checks if the key exists in any registered save system.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists in any system; false otherwise.</returns>
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

        /// <summary>
        /// Checks if the key exists in a specific save system.
        /// </summary>
        /// <typeparam name="TSystem">The type of the save system.</typeparam>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists in that system; false otherwise.</returns>
        public bool ExistsIn<TSystem>(string key) where TSystem : ISaveSystem<T>
        {
            if (saveSystems.TryGetValue(typeof(TSystem), out var system))
            {
                return system.Exists(key);
            }
            return false;
        }

        // -----------------------------
        // PART 4: Delete
        // -----------------------------

        /// <summary>
        /// Deletes the data associated with the key from all systems.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        public void Delete(string key)
        {
            foreach (var system in saveSystems.Values)
            {
                system.Delete(key);
            }
        }

        /// <summary>
        /// Deletes the data associated with the key from a specific system.
        /// </summary>
        /// <typeparam name="TSystem">The save system to delete from.</typeparam>
        /// <param name="key">The key to delete.</param>
        public void DeleteFrom<TSystem>(string key) where TSystem : ISaveSystem<T>
        {
            if (saveSystems.TryGetValue(typeof(TSystem), out var system))
            {
                system.Delete(key);
            }
        }

        /// <summary>
        /// Deletes all data from all registered save systems.
        /// </summary>
        public void DeleteAll()
        {
            foreach (var system in saveSystems.Values)
            {
                system.DeleteAll();
            }
        }

        /// <summary>
        /// Deletes all data from a specific save system.
        /// </summary>
        /// <typeparam name="TSystem">The type of save system to clear.</typeparam>
        public void DeleteAllFrom<TSystem>() where TSystem : ISaveSystem<T>
        {
            if (saveSystems.TryGetValue(typeof(TSystem), out var system))
            {
                system.DeleteAll();
            }
        }

        // -----------------------------
        // PART 5: Debug
        // -----------------------------

        /// <summary>
        /// Returns a string summary of the registered systems.
        /// </summary>
        public override string ToString()
        {
            return $"SaveManager<{typeof(T).Name}> with systems: [{string.Join(", ", saveSystems.Keys)}]";
        }

        /// <summary>
        /// Returns the list of all registered system types.
        /// </summary>
        public IEnumerable<Type> GetRegisteredSystems()
        {
            return saveSystems.Keys;
        }
    }
}
