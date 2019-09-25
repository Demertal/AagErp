using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
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
        /// <param name="cts">Токен для отмены операции</param>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        /// <returns>Кол-во данных</returns>
        Task<int> GetCountAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1);

        /// <summary>
        /// Проверяет на существоание объект
        /// </summary>
        /// <param name="cts">Токен для отмены операции</param>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        Task<bool> AnyAsync(CancellationToken cts = new CancellationToken(), ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1);

        /// <summary>
        /// Возвращает коллекцию данных
        /// </summary>
        /// <param name="cts">Токен для отмены операции</param>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="include">Список полей для include</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="take">Кол-во записей, которые нужно выбрать</param>
        /// <returns>Коллекция данных</returns>
        Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cts = new CancellationToken(),
            ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0,
            int take = -1,
            params (Expression<Func<TEntity, Object>> include, Expression<Func<Object, Object>>[] thenInclude)[]
                include);

        /// <summary>
        /// Получает объект соответсвующий условию
        /// </summary>
        /// <param name="cts">Токен для отмены операции</param>
        /// <param name="where">Фильтр для выборки, если null получает все данные</param>
        /// <param name="order">Список полей для сортировки</param>
        /// <param name="skip">Кол-во записей, которые нужно пропустить</param>
        /// <param name="include">Список полей для include</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(CancellationToken cts = new CancellationToken(),
            ISpecification<TEntity> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0,
            params (Expression<Func<TEntity, Object>> include, Expression<Func<Object, Object>>[] thenInclude)[]
                include);

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="cts">Токен для отмены операции</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(int id, CancellationToken cts = new CancellationToken());

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="cts">Токен для отмены операции</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(long id, CancellationToken cts = new CancellationToken());

        /// <summary>
        /// Получает объект по id
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="cts">Токен для отмены операции</param>
        /// <returns>Полученный объект</returns>
        Task<TEntity> GetItemAsync(Guid id, CancellationToken cts = new CancellationToken());

        /// <summary>
        /// Создает объект
        /// </summary>
        /// <param name="item">Объект для создания</param>
        Task CreateAsync([NotNull]TEntity item, CancellationToken cts = new CancellationToken());

        /// <summary>
        /// Обновляет объект
        /// </summary>
        /// <param name="item">Объект для обновления</param>
        Task UpdateAsync([NotNull]TEntity item, CancellationToken cts = new CancellationToken());

        /// <summary>
        /// Удаляет объект
        /// </summary>
        /// <param name="item">Удаляемый объект</param>
        Task DeleteAsync([NotNull]TEntity item, CancellationToken cts = new CancellationToken());
    }
}
