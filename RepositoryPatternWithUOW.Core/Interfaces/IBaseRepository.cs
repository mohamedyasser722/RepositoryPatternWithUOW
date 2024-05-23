using RepositoryPatternWithUOW.Core.Consts;
using System.Linq.Expressions;

namespace RepositoryPatternWithUOW.Api.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();

        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip, 
            Expression<Func<T,object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] incldes = null);
        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        public void DeleteRange(IEnumerable<T> entities);
    }
}
