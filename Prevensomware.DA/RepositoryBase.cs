using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using Prevensomware.Dto;

namespace Prevensomware.DA
{
    public class RepositoryBase<T>: IRepository<T> where T : DtoBase
    {
        protected  ISession Session { get; set; } 
        public RepositoryBase()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(T).Assembly);

            var sessions = cfg.BuildSessionFactory();
            Session = sessions.OpenSession();
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
    }
}
