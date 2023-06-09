
using HotelAdministrator.Models;

namespace UI.Interfaces;

public interface IConsoleManager<TEntity> where TEntity : BaseEntity
{
    void PerformOperations();
    
    IEnumerable<TEntity> GetAll();

    TEntity GetById(Guid id);

    TEntity GetByPredicate(Func<TEntity, bool> predicate);

    void Add(TEntity obj);

    void Update(Guid id, TEntity updateObj);

    void Delete(Guid id);
}