using Core;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Sepecifcations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context) {
            _context = context;
        }

     

        public async Task Add(T entity)
       => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
        =>  _context.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

      

        public async Task<T> GetByIdAsync(int? id)
       => await _context.Set<T>().FindAsync(id);

        public void Update(T entity)
         => _context.Set<T>().Update(entity);
        public async Task<T> GetEntityWithSpecificationsAsync(ISepecifcations<T> specs)
            => await ApplySepecifications(specs).FirstOrDefaultAsync();
      // => await SepecifcationsEvaluter<T>GetQuery(_context.Set<T>().AsQueryable, specs);
          public async Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISepecifcations<T> specs)
           => await ApplySepecifications(specs).ToListAsync();

        private IQueryable<T> ApplySepecifications(ISepecifcations<T>specs)
             => SepecifcationsEvaluter<T>.GetQuery(_context.Set<T>().AsQueryable(),specs);

        public async Task<int> CountAsync(ISepecifcations<T> sepecifcations)
      => await ApplySepecifications(sepecifcations).CountAsync();   
        // => await _context.Set<T>().FindAsync(id);
        //{
        //    if (typeof(T) == typeof(Product))
        //    {
        //        await _context.Products.Include(x => x.ProductBrand)
        //            .Include(x =>x.ProductType)
        //            .FirstOrDefaultAsync(x=> x.Id == id);

        //    }
        //    return await _context.Set<T>().FindAsync(id);
        //}

    }
}
