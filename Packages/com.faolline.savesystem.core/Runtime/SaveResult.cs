using System;
using System.Collections.Generic;
using System.Linq;

namespace SaveSystem
{
    public class SaveResult<T>
    {
        private readonly Dictionary<Type, T> _data;

        public SaveResult(Dictionary<Type, T> data)
        {
            _data = data;
        }

        public bool TryGet<TSystem>(out T value) where TSystem : ISaveSystem<T>
        {
            return _data.TryGetValue(typeof(TSystem), out value);
        }

        public T Get<TSystem>() where TSystem : ISaveSystem<T>
        {
            return _data.TryGetValue(typeof(TSystem), out var value) ? value : default;
        }

        public bool Has<TSystem>() where TSystem : ISaveSystem<T>
        {
            return _data.ContainsKey(typeof(TSystem));
        }

        public override string ToString()
        {
            return $"SaveResult<{typeof(T).Name}>[{string.Join(", ", _data.Keys.Select(k => k.Name))}]";
        }
    }
}
