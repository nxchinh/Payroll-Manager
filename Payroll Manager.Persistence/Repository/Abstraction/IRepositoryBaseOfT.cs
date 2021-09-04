using System;
using System.Linq;
using System.Linq.Expressions;
using Payroll_Manager.Entity.Abstract;

namespace Payroll_Manager.Persistence.Repository.Abstraction
{
    public interface IRepositoryBase<T> where T : class, IEntity
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
