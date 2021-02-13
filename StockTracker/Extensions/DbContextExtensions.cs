using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using StockTracker.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockTracker.Extensions
{
    public static class DbContextExtensions
    {
        public static object ResolveDbSet<T>(this DbContext source)
        {
            return source.ResolveDbSet(typeof(T));
        }
        public static object ResolveDbSet(this DbContext source, Type type)
        {
            var dbSetType = typeof(DbSet<>).MakeGenericType(type);
            foreach (var method in source.GetType().GetMethods())
            {
                if (method.ReturnType.IsAssignableFrom(dbSetType))
                {
                    return method.Invoke(source, null);
                }
            }

            throw new NotImplementedException("Unable to find DbSet for type: " + dbSetType);
        }

        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> source) where T : class
        {
            return source.IncludeTypes(new[] { typeof(IEnumerable), typeof(IEntity) });
        }

        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> source, Type[] types, Type type, string ancestors = null, int level = 1) where T : class
        {
            var maxDepth = 2;
            if (level > maxDepth) return source;

            foreach (var prop in type.GetProperties())
            {
                var interfaces = prop.PropertyType.GetInterfaces();
                var genericArguments = prop.PropertyType.GetGenericArguments();

                if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(byte[]) ||
                    !interfaces.Intersect(types).Any() ||
                    level > 1 && interfaces.Intersect(new[] { typeof(IEnumerable) }).Any() ||
                    prop.GetCustomAttribute(typeof(NotMappedAttribute)) != null)
                    continue;

                var propertyName = ancestors != null ? $"{ancestors}.{prop.Name}" : prop.Name;
                source = source.Include(propertyName);
                // traverse tree...
                if (interfaces.Intersect(new[] { typeof(IEnumerable) }).Any())
                {
                    if (genericArguments.Length > 0 &&
                        genericArguments[0] != typeof(T))
                        source = source.IncludeAll(types, prop.PropertyType.GetGenericArguments()[0], propertyName,
                            level + 1);
                }
                else
                {
                    if (prop.PropertyType != typeof(T))
                        source = source.IncludeAll(types, prop.PropertyType, propertyName, level + 1);
                }
            }

            return source;
        }

        public static IQueryable<T> IncludeType<T>(this IQueryable<T> source, Type type) where T : class
        {
            return source.IncludeTypes(new[] { type });
        }

        public static IQueryable<T> IncludeTypes<T>(this IQueryable<T> source, Type[] types) where T : class
        {
            return IncludeAll<T>(source, types, typeof(T));
        }

        public static void UpdateEntity<T>(this DbContext context, T updatedEntity) where T : Entity
        {
            DbSet<T> matchedSet = context.ResolveDbSet(typeof(T)) as DbSet<T>;
            context.Entry(matchedSet).CurrentValues.SetValues(updatedEntity);
        }
    }
}
