using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Infrastructure.Repos
{
    public class Repository<TEntity>(CatalogContext context) : IRepository<TEntity> where TEntity : class
    {
        private readonly CatalogContext _context = context;

        public IQueryable<TEntity> GetAll(bool trackChanges) =>
            trackChanges ? _context.Set<TEntity>() : _context.Set<TEntity>().AsNoTracking();

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, bool trackChanges) =>
            trackChanges ? _context.Set<TEntity>().Where(predicate) : _context.Set<TEntity>().Where(predicate).AsNoTracking();

        public void Add(TEntity entity) => 
            _context.Set<TEntity>().Add(entity);

        public void Update(TEntity entity) => 
            _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);
    }
}
