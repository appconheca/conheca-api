using ConhecaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Repositories
{
    public interface IRepository<T> where T : IDomainObject, new()
    {
        T Create(T domainObject);

        void Update(object pk, T domainObject);

        void Delete(object pk);

        public List<T> Read();
        
        public T Read(object pk);

    }
}
