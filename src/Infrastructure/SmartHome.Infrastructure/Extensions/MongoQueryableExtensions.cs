using System;
using MongoDB.Driver.Linq;

namespace SmartHome.Infrastructure.Extensions
{
    public static class MongoQueryableExtensions
    {
        public static IMongoQueryable<T> AddFilter<T>(
            this IMongoQueryable<T> queryable,
            bool condition,
            Func<IMongoQueryable<T>, IMongoQueryable<T>> filter)
        {
            return condition ? filter(queryable) : queryable;
        }
    }
}