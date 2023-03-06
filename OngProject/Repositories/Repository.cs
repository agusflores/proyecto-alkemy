using Microsoft.EntityFrameworkCore;
using OngProject.DataAccess;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly OngDbContext _context;

        public Repository(OngDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(T entity)
        {
            T existing = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existing == null)
                return false;
            else {

                //_context.Set<T>().Remove(existing);
                entity.IsDeleted = true;
                this.Update(entity);
                return true;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync<T>(x => x.Id == id);
        }

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
