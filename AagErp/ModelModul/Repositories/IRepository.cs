using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ModelModul.Annotations;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Получает кол-во данных в коллекции
        /// </summary>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        /// <returns>Кол-во данных</returns>
        Task<int> GetCountAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1);

        /// <summary>
        /// Проверяет на существоание объект
        /// </summary>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        Task<bool> AnyAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1);

        /// <summary>
        /// Возвращает коллекцию данных
        /// </summary>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="include">Список полей для include</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        /// <returns>Коллекция данных</returns>
        Task<IEnumerable<TEntity>> GetListAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1, params Expression<Func<TEntity, Object>>[] include);

        /// <summary>
        /// Получает объект соответсвующий условию
        /// </summary>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0);

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(int id);

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(long id);

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(Guid id);

        /// <summary>
        /// Создает объект
        /// </summary>
        /// <param name="item">Объект для создания</param>
        Task CreateAsync([NotNull]TEntity item);

        /// <summary>
        /// Обновляет объект
        /// </summary>
        /// <param name="item">Объект для обновления</param>
        Task UpdateAsync([NotNull]TEntity item);

        /// <summary>
        /// Удаляет объект
        /// </summary>
        /// <param name="item">Удаляемый объект</param>
        Task DeleteAsync([NotNull]TEntity item);
    }
}
