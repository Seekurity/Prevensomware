using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prevensomware.Logic
{
    public interface IBo<T>
    {
        void Save(T obj);
        void Remove(T obj);
        T Get(int oid);
        IEnumerable<T> GetList();

    }
}
