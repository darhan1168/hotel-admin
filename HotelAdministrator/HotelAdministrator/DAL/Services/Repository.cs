using HotelAdministrator.DAL.Interfaces;
using HotelAdministrator.Models;
using Newtonsoft.Json;

namespace HotelAdministrator
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;

        public Repository(string? filePath = null)
        {
            _filePath = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{typeof(T).Name}.json");
            EnsureFileExists();
        }
        public List<T> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var items = GetAllItems();
                var pagedItems = items
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return pagedItems;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get all items. Exception: {ex.Message}");
            }
        }

        public T GetById(Guid id)
        {
            return GetByPredicate(item => item.Id.Equals(id));
        }

        public T GetByPredicate(Func<T, bool> predicate)
        {
            try
            {
                var item = GetAllItems().FirstOrDefault(predicate);

                if (item is null)
                {
                    throw new Exception("Item not found.");
                }

                return item;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get item. Exception: {ex.Message}");
            }
        }

        public void Add(T obj)
        {
            try
            {
                var items = GetAllItems();
                items.Add(obj);
                
                SaveItems(items);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add item. Exception: {ex.Message}");
            }
        }

        public void Update(Guid id, T updateObj)
        {
            try
            {
                var items = GetAllItems();
                int index = items.FindIndex(item => item.Id.Equals(id));

                if (index == -1)
                {
                    throw new Exception("Object with the specified Id not found.");
                }

                items[index] = updateObj;
                SaveItems(items);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update item. Exception: {ex.Message}");
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var items = GetAllItems();
                int index = items.FindIndex(item => item.Id.Equals(id));
            
                if (index == -1)
                {
                    throw new Exception("Object with the specified Id not found.");
                }
            
                items.RemoveAt(index);
                SaveItems(items);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete item. Exception: {ex.Message}");
            }
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                WriteToFile(new List<T>());
            }
        }

        private List<T> GetAllItems()
        {
            try
            {
                using StreamReader file = File.OpenText(_filePath);
                using JsonTextReader reader = new JsonTextReader(file);
                JsonSerializer serializer = new JsonSerializer();

                return serializer.Deserialize<List<T>>(reader);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get items from the file. Exception: {ex.Message}");
            }
        }

        private void WriteToFile(List<T> items)
        {
            try
            {
                using StreamWriter file = File.CreateText(_filePath);
                using JsonTextWriter writer = new JsonTextWriter(file)
                {
                    Formatting = Formatting.Indented
                };

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, items);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write items to the file. Exception: {ex.Message}");
            }
        }

        private void SaveItems(List<T> items)
        {
            WriteToFile(items);
        }
    }
}