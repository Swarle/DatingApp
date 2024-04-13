using System.Linq.Expressions;

namespace DatingApp.DAL.Specification.Infrastructure
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Expression { get; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        public List<string> IncludeString { get; }
        public Expression<Func<TEntity, object>>? OrderBy { get; }

        bool IsSatisfied(TEntity obj);
    }
}
