using KeyWordsWebCrawlerDomain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace KeyWordsWebCrawlerDataAcces.Common
{
    public class EFRepository<TObject> : IBaseRepository<TObject> where TObject : class, IEntity
    {
        protected DbContext Context = null;
        private bool shareContext = false;
        protected IQueryable<TObject> QueryableDbSet;

        public EFRepository(DbContext context)
        {
            Context = context;
            shareContext = true;
            QueryableDbSet = DbSet.AsQueryable();
        }

        protected DbSet<TObject> DbSet
        {
            get
            {
                return Context.Set<TObject>();
            }
        }

        public void Dispose()
        {
            if (shareContext && (Context != null))
                Context.Dispose();
        }

        public void Refresh(TObject entity)
        {
            Context.Entry<TObject>(entity).Reload();
        }

        public virtual IQueryable<TObject> All()
        {
             return QueryableDbSet;
        }

        public IQueryable<TObject> GetAll(params Expression<Func<TObject, object>>[] includeExpressions)
        {
            return includeExpressions.Aggregate<Expression<Func<TObject, object>>, IQueryable<TObject>>
             (All(), (current, expression) => current.Include(expression));
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return All().Where(predicate);
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate, params Expression<Func<TObject, object>>[] includeExpressions)
        {
            return GetAll(includeExpressions).Where(predicate);
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;

            var _resetSet = filter != null ? All().Where(filter) : All();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();

            return _resetSet.AsQueryable();
        }

        public IQueryable<TObject> Filter<Key>(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;

            var _resetSet = filter != null ? All().Where(filter) : All().AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) :
                _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return All().Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            var entity = DbSet.Find(keys);
            return entity;
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return All().FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!shareContext)
                Context.SaveChanges();
            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual void Delete(TObject TObject)
        {
            DbSet.Remove(TObject);

            Context.SaveChanges();
        }

        public virtual bool DeleteRange(IEnumerable<TObject> TObjects)
        {
            DbSet.RemoveRange(TObjects);

            return (Context.SaveChanges() > 0);
        }

        public virtual int Update(TObject TObject)
        {
            var entry = Context.Entry(TObject);

            //set the entire entity attached only if it's detached
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(TObject);
                entry.State = EntityState.Modified;
            }

            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual int Update(TObject TObject, string userName, string dnb = null)
        {
            var entry = Context.Entry(TObject);

            //set the entire entity attached only if it's detached
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(TObject);
                entry.State = EntityState.Modified;
            }

            if (!shareContext)
                return Context.SaveChanges();
            return 0;
        }

        public virtual void Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            DeleteRange(objects);
        }

        #region helpers

        private void SetPropertyModified(DbEntityEntry<TObject> entry, string property, bool modified)
        {
            if (entry.Entity.GetType().GetProperty(property) != null)
            {
                entry.Property(property).IsModified = modified;
            }
        }

        private void SetStringProperty(DbEntityEntry<TObject> entry, string property, string value)
        {
            if (entry.Entity.GetType().GetProperty(property) != null)
            {
                entry.Property(property).CurrentValue = value;
            }
        }

        private void SetDateTimeProperty(DbEntityEntry<TObject> entry, string property, DateTime value)
        {
            if (entry.Entity.GetType().GetProperty(property) != null)
            {
                entry.Property(property).CurrentValue = value;
            }
        }

        private static class ReflectionUtility
        {
            public static string GetPropertyName<T>(Expression<Func<T>> expression)
            {
                MemberExpression body = (MemberExpression)expression.Body;
                return body.Member.Name;
            }
        }

        #endregion
    }
}
