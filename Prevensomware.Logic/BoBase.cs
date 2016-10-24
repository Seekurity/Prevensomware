using System.Collections.Generic;
using Prevensomware.DA;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class BoBase<T> : IBo<T> where T : DtoBase
    {
        private IRepository<T> _repository;
        protected IRepository<T> Repository
        {
            get
            {
                if(_repository != null) return _repository;
                _repository = new RepositoryBase<T>();
                return _repository;
            }
        }

        public virtual void Save(T obj)
        {
            Repository.CreateOrUpdate(obj);
        }

        public virtual void Remove(T obj)
        {
            Repository.Remove(obj);
        }

        public virtual T Get(int oid)
        {
            return Repository.Get(oid);
        }

        public virtual IEnumerable<T> GetList()
        {
            return Repository.GetList();
        }

        public virtual void RemoveList(IEnumerable<T> objList)
        {
            Repository.RemoveList(objList);
        }
    }
}
