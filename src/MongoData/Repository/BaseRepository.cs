using Dasync.Collections;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoData.Repository
{
    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class BaseRepository<T, TKey> : IRepository<T, TKey>, IRepositoryAsync<T, TKey>
        where T : IEntity<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            Collection = unitOfWork.Database.GetCollection<T>(GetCollectionName());
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        public IMongoCollection<T> Collection { get; }

        /// <summary>
        /// Returns a IQueryable<T>.
        /// </summary>
        /// <returns>IQueryable T.</returns>
        public virtual IQueryable<T> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// Returns a IQueryable<T> allowing diskuse to handle large datasets.
        /// </summary>
        /// <returns>IQueryable T.</returns>
        public virtual IQueryable<T> AsQueryableLargeDataSet()
        {
            return Collection.AsQueryable<T>(new AggregateOptions { AllowDiskUse = true });
        }

        /// <summary>
        /// filter for collection
        /// </summary>
        public FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;

        /// <summary>
        /// updater for collection
        /// </summary>
        public UpdateDefinitionBuilder<T> Updater => Builders<T>.Update;

        /// <summary>
        /// projector for collection
        /// </summary>
        public ProjectionDefinitionBuilder<T> Projector => Builders<T>.Projection;

        /// <summary>
        /// indexer for collection
        /// </summary>
        public IndexKeysDefinitionBuilder<T> Indexer => Builders<T>.IndexKeys;

        public string CreateIndexAscending(Expression<Func<T, object>> field)
        {
            T temp = default;
            if (!Any())
            {
                temp = Add(new T());
            }

            var indexModel = new CreateIndexModel<T>(Indexer.Ascending(field));
            var result = Collection.Indexes.CreateOne(indexModel);

            if (temp != null)
            {
                Delete(temp);
            }

            return result;
        }

        private static FilterDefinition<T> GetIdFilter(TKey id)
        {
            return Builders<T>.Filter.Eq(s => s.Id, id);
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual void Add(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual Task AddAsync(IEnumerable<T> entities)
        {
            return Collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual T Add(T entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public async virtual Task<T> AddAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count()
        {
            //var filter = Builders<T>.Filter.Empty;
            return Collection.CountDocuments(x => true);
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual Task<long> CountAsync()
        {
            return Collection.CountDocumentsAsync(t => true);
        }

        public long EstimatedCount()
        {
            return Collection.EstimatedDocumentCount();
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            Collection.DeleteMany<T>(predicate);
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.DeleteManyAsync<T>(predicate);
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            Collection.DeleteOne(GetIdFilter(entity.Id));
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual Task<DeleteResult> DeleteAsync(T entity)
        {
            return DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual void Delete(TKey id)
        {
            Collection.DeleteOne(GetIdFilter(id));
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public async virtual Task<DeleteResult> DeleteAsync(TKey id)
        {
            return await Collection.DeleteOneAsync(GetIdFilter(id));
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            Collection.DeleteMany(x => true);
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public virtual Task<DeleteResult> DeleteAllAsync()
        {
            return Collection.DeleteManyAsync<T>(t => true);
        }

        public virtual void Drop()
        {
            Collection.Database.DropCollection(Collection.CollectionNamespace.CollectionName);
        }

        public virtual Task DropAsync()
        {
            return Collection.Database.DropCollectionAsync(Collection.CollectionNamespace.CollectionName);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        public virtual bool Any()
        {
            return Collection.AsQueryable().Any();
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        public virtual Task<bool> AnyAsync()
        {
            return Collection.AsQueryable().AnyAsync();
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return Collection.AsQueryable().Any(predicate);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.CountDocumentsAsync(predicate) > 0;
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual T GetById(TKey id)
        {
            return Collection.Find(GetIdFilter(id)).SingleOrDefault();
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual Task<T> GetByIdAsync(TKey id)
        {
            return Collection.Find(GetIdFilter(id)).SingleAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).ToEnumerable();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageIndex, int size)
        {
            return Collection.Find(predicate).Skip(pageIndex * size).Limit(size).ToEnumerable();
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var cursor = await Collection.FindAsync(predicate);
            return cursor.ToEnumerable();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageIndex, int size)
        {
            var findOptions = new FindOptions<T>
            {
                Limit = 1,
                Skip = pageIndex * size
            };
            var cursor = await Collection.FindAsync(predicate, findOptions).ConfigureAwait(false);
            return cursor.ToEnumerable();
        }


        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };
            Parallel.ForEach(entities, parallelOptions, entity =>
            {
                Update(entity);
            });
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual Task UpdateAsync(IEnumerable<T> entities)
        {
            return entities.ParallelForEachAsync(async entity =>
            {
                await UpdateAsync(entity).ConfigureAwait(false);
            }, maxDegreeOfParallelism: 4);
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity)
        {
            Collection.ReplaceOne(GetIdFilter(entity.Id), entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public async virtual Task<T> UpdateAsync(T entity)
        {
            if (entity.Id == null)
            {
                await AddAsync(entity);
            }
            else
            {
                await Collection.ReplaceOneAsync(GetIdFilter(entity.Id), entity, new ReplaceOptions { IsUpsert = true }).ConfigureAwait(false);
            }
            return entity;
        }

        private static string GetCollectionName()
        {
            var collectionName = typeof(T).GetTypeInfo().BaseType == typeof(object)
                ? GetCollectioNameFromInterface()
                : GetCollectionNameFromType(typeof(T));

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }

            return collectionName;
        }

        private static string GetCollectioNameFromInterface()
        {
            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = typeof(T).GetTypeInfo().GetCustomAttribute<CollectionNameAttribute>();
            return att?.Name ?? typeof(T).Name;
        }

        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = entitytype.GetTypeInfo().GetCustomAttribute<CollectionNameAttribute>();
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = att.Name;
            }
            else
            {
                //if (typeof(Entity).GetTypeInfo().IsAssignableFrom(entitytype))
                //{
                //    while (entitytype.GetTypeInfo().BaseType != typeof(Entity))
                //    {
                //        entitytype = entitytype.GetTypeInfo().BaseType;
                //    }
                //}
                collectionname = entitytype.Name;
            }

            return collectionname;
        }

        #region IQueryable<T>

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of IQueryable is executed.
        /// </summary>
        public virtual Type ElementType => Collection.AsQueryable().ElementType;

        /// <summary>
        /// Gets the expression tree that is associated with the instance of IQueryable.
        /// </summary>
        public virtual Expression Expression => Collection.AsQueryable().Expression;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public virtual IQueryProvider Provider => Collection.AsQueryable().Provider;

        #endregion
    }

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class BaseRepository<T> : BaseRepository<T, string>, IRepository<T>, IRepositoryAsync<T>
        where T : IEntity<string>, new()
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}