using System.Collections.Generic;
using NHibernate;
using Prevensomware.Dto;

namespace Prevensomware.DA
{
    public class RepositoryBase<T>: IRepository<T> where T : DtoBase
    {
        protected ISession Session
        {
            get { return SessionManager.Session; }
        }

        public void CreateOrUpdate(T obj)
        {
            Session.Save(obj);
            Session.Flush();
        }

        public void Remove(T obj)
        {
            Session.Delete(obj);
            Session.Flush();
        }


        public T Get(int oid)
        {
            return Session.QueryOver<T>().Where(x => x.Oid == oid).SingleOrDefault();
        }

        public IEnumerable<T> GetList()
        {
            return Session.QueryOver<T>().List();
        }

        public void RemoveList(IEnumerable<T> objList)
        {
            using (var tx = Session.BeginTransaction())
            {
                foreach (var obj in objList)
                    Session.Delete(obj);
                tx.Commit();
            }
        }
    }
}
