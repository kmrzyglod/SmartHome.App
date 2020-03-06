using System;
using System.Linq;

namespace SmartHome.Application.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> AddFilter<T>(
            this IQueryable<T> queryable,
            bool condition,
            Func<IQueryable<T>, IQueryable<T>> filter)
        {
            return condition ? filter(queryable) : queryable;
        }

        public static IQueryable<T> AddPagination<T>(
            this IQueryable<T> queryable,
            int pageNumber,
            int pageSize)
        {
            return queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}