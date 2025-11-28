using Warehouse.Data;
using Warehouse.Domain;
using Warehouse.Interfaces;

namespace Warehouse.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly List<T> _items;
        protected readonly CsvDataManager _csv;

        protected BaseRepository(CsvDataManager csv)
        {
            _items = new List<T>();
            _csv = csv;
            LoadData();
        }

        protected abstract void LoadData();
        protected abstract void SaveData();

        public virtual void Add(T entity)
        {
            int maxId = _items.Any() ? _items.Max(i => i.Id) : 0;
            entity.Id = maxId + 1;
            _items.Add(entity);
            SaveData();
            Console.WriteLine($"Added: {entity.GetEntityInfo()}");
        }

        public virtual void Update(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException($"Entity with ID {entity.Id} not found");

            int index = _items.IndexOf(existing);
            _items[index] = entity;
            SaveData();
            Console.WriteLine($"Updated: {entity.GetEntityInfo()}");
        }

        public virtual void Delete(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing == null)
                return;

            existing.Deactivate();
            SaveData();
            Console.WriteLine($"Deleted (soft): {existing.GetEntityInfo()}");
        }

        public virtual void HardDelete(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing == null)
                return;

            _items.Remove(existing);
            SaveData();
            Console.WriteLine($"Deleted (hard): {existing.GetEntityInfo()}");
        }

        public virtual T? GetById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _items.Where(i => i.IsActive).ToList();
        }

        public virtual int Count()
        {
            return _items.Count(i => i.IsActive);
        }

        protected IEnumerable<T> GetAllIncludingInactive()
        {
            return _items.ToList();
        }
    }
}
