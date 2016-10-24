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

        public virtual void CreateOrUpdate(T obj)
        {
            Session.Save(obj);
            Session.Flush();
        }

        public virtual void Remove(T obj)
        {
            Session.Delete(obj);
            Session.Flush();
        }


        public virtual T Get(int oid)
        {
            return Session.QueryOver<T>().Where(x => x.Oid == oid).SingleOrDefault();
        }

        public virtual IEnumerable<T> GetList()
        {
            return Session.QueryOver<T>().List();
        }

        public virtual void RemoveList(IEnumerable<T> objList)
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
