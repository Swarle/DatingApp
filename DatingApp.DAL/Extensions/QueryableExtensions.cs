using DatingApp.DAL.Specification.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySpecification<TEntity>(this IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification) where TEntity : class
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
            

        return query;
    }
}