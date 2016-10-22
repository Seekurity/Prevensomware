using System;
using System.Collections.Generic;

namespace Prevensomware.DA
{
    public interface IRepository<T>
    { 
        void CreateOrUpdate(T obj);
        void Remove(T obj);
        T Get(int oid);
        IEnumerable<T> GetList();
        void RemoveList(IEnumerable<T> objList);

    }
}
