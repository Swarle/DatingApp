using System.Linq.Expressions;
using DatingApp.DAL.Extensions;

namespace DatingApp.DAL.Specification.Infrastructure
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Expression { get; private set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = [];
        public List<string> IncludeString { get; private set; } = [];
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }


        protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
        {
            Expression = expression;
        }

        protected BaseSpecification()
        {
        
        }

        protected virtual BaseSpecification<TEntity> AddExpression(Expression<Func<TEntity, bool>> expression)
        {
            Expression = Expression == null ? expression 
                : Expression.And(expression);

            return this;
        }

        protected virtual BaseSpecification<TEntity> AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;

            return this;
        }

        protected virtual BaseSpecification<TEntity> AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);

            return this;
        }

        protected virtual BaseSpecification<TEntity> AddInclude(string include)
        {
            IncludeString.Add(include);

            return this;
        }
        
        public virtual bool IsSatisfied(TEntity obj)
        {
            bool result = Expression!.Compile().Invoke(obj);

            return result;
        }

    }
}
