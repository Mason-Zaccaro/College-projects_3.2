using WebApplication1.Interfaces;

namespace WebApplication1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _data;
        private int _nextId = 1;

        public Repository()
        {
            _data = new List<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_data.AsEnumerable());
        }

        public Task<T?> GetByIdAsync(int id)
        {
            var entity = _data.FirstOrDefault(x => GetId(x) == id);
            return Task.FromResult(entity);
        }

        public Task<T> AddAsync(T entity)
        {
            SetId(entity, _nextId++);
            _data.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            var existingIndex = _data.FindIndex(x => GetId(x) == GetId(entity));
            if (existingIndex >= 0)
            {
                _data[existingIndex] = entity;
            }
            return Task.FromResult(entity);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var entity = _data.FirstOrDefault(x => GetId(x) == id);
            if (entity != null)
            {
                _data.Remove(entity);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private int GetId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            return (int)(property?.GetValue(entity) ?? 0);
        }

        private void SetId(T entity, int id)
        {
            var property = typeof(T).GetProperty("Id");
            property?.SetValue(entity, id);
        }
    }
}