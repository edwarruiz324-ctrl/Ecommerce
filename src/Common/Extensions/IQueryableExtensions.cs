using Common.Models;

namespace Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PaginationFilter filter)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var take = filter.PageSize;

            return query.Skip(skip).Take(take);
        }
    }
}
