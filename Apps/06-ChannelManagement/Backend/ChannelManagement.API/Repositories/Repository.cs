using System.Linq.Expressions;

namespace ChannelManagement.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly List<T> _context;

        public Repository(List<T> context)
        {
            _context = context;
        }

        public virtual Task<T> GetByIdAsync(int id)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
                throw new InvalidOperationException("Entity does not have an Id property");

            var entity = _context.FirstOrDefault(e => (int)property.GetValue(e) == id);
            return Task.FromResult(entity);
        }

        public virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_context.AsEnumerable());
        }

        public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var result = _context.AsQueryable().Where(predicate).ToList();
            return Task.FromResult(result.AsEnumerable());
        }

        public virtual Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var result = _context.AsQueryable().SingleOrDefault(predicate);
            return Task.FromResult(result);
        }

        public virtual Task AddAsync(T entity)
        {
            _context.Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual void Update(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null) return;

            var id = (int)property.GetValue(entity);
            var existing = _context.FirstOrDefault(e => (int)property.GetValue(e) == id);
            if (existing != null)
            {
                var index = _context.IndexOf(existing);
                _context[index] = entity;
            }
        }

        public virtual void Remove(T entity)
        {
            _context.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                _context.Remove(entity);
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(_context.AsQueryable().Any(predicate));
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            var count = predicate == null ? _context.Count : _context.AsQueryable().Count(predicate);
            return Task.FromResult(count);
        }
    }
}