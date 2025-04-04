using System;
using System.Collections.Generic;
using System.Linq;

namespace SaveSystem
{
    public class LoadResult<T>
    {
        private readonly Dictionary<Type, T> _data;

        public LoadResult(Dictionary<Type, T> data)
        {
            _data = data;
        }

        public bool TryGet<TSystem>(out T value) where TSystem : ISaveSystem<T>
        {
            return _data.TryGetValue(typeof(TSystem), out value);
        }

        public bool Has<TSystem>() where TSystem : ISaveSystem<T>
        {
            return _data.ContainsKey(typeof(TSystem));
        }

        public override string ToString()
        {
            return $"LoadResult<{typeof(T).Name}>[{string.Join(", ", _data.Keys.Select(k => k.Name))}]";
        }
    }
}
