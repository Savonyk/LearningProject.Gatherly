using Gatherly.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Gatherly.Persistence.Specifications;
internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if(specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        specification.IncludeExpressions.Aggregate(
            queryable, 
            (current, includeExpression) 
            => current.Include(includeExpression));

        if(specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if(specification.OrderByDescedingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescedingExpression);
        }

        if(specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}
