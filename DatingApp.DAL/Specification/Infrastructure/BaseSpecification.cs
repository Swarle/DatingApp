using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.DAL.Specification.Infrastructure
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Expression { get; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; } = [];
        public List<string> IncludeString { get; set; } = [];



        protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
        {
            Expression = expression;
        }

        protected BaseSpecification()
        {

        }

        protected virtual void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }

        protected virtual void AddInclude(string include)
        {
            IncludeString.Add(include);
        }
        
        public virtual bool IsSatisfied(TEntity obj)
        {
            bool result = Expression!.Compile().Invoke(obj);

            return result;
        }

    }
}
