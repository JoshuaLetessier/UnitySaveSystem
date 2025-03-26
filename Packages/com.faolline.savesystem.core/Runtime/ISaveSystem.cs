using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveSystem
{
    public interface ISaveSystem<T>
    {
        void Save(string key, T data);
        T Load(string key);
        void Delete(string key);
        void DeleteAll();
        bool Exists(string key);
    }
}
