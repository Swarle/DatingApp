using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.DAL.Specification.Infrastructure
{
    public static class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
            ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            if (specification.Expression != null)
            {
                query = query.Where(specification.Expression);
            }

            if (specification.IncludeExpressions.Count > 0)
            {
                query = specification.IncludeExpressions.Aggregate(query, (current, include) =>
                    current.Include(include));
            }

            if (specification.IncludeString.Count > 0)
            {
                query = specification.IncludeString
                    .Aggregate(query,
                        (current, include) => current.Include(include));
            }

            if (specification.Paging != null)
            {
                query = query.Skip(specification.Paging.Skip)
                    .Take(specification.Paging.Take);
            }

            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

    }
}
