using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Repositories.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Insert(T entity);
        public Task<bool> Delete(T entity);
        void Update(T entity);

        Task Save();

    }
}
