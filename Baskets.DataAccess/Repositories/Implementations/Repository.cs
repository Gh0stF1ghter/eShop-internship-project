﻿using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class Repository<TEntity>(IMongoClient client, IOptions<BasketDatabaseSettings> options, string collectionName) : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection = client.GetDatabase(options.Value.DatabaseName)
                                                                       .GetCollection<TEntity>(collectionName);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
            await _collection.Find(t => true)
                       .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.Find(condition).ToListAsync(cancellationToken);

        public void Add(TEntity entity) => 
            _collection.InsertOne(entity);

        public void Update(TEntity entity) =>
            _collection.ReplaceOne(t => t.Equals(entity), entity);

        public void Delete(TEntity entity) => 
            _collection.DeleteOne(t => t.Equals(entity));
    }
}