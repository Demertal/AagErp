using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ModelModul.Repositories
{
    public enum SortingTypes
    {
        ASC,
        DESC
    }

    public static class SqlRepositoryUtility
    {
        private static LambdaExpression GenerateSelector<TEntity>(string propertyName, out Type resultType) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                string[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException("Свойство" + childProperties[0] + " не найдено"));
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property ?? throw new InvalidOperationException("Свойство" + childProperties[i] + " не найдено"));
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException("Свойство" + propertyName + " не найдено"));
            }
            resultType = property.PropertyType;
            return Expression.Lambda(propertyAccess, parameter);
        }
        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out Type selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                            new Type[] { type, selectorResultType },
                            source.Expression, Expression.Quote(selector));
            return resultExp;
        }

        #region OrderBy
        
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderByDesc<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> ThenByDesc<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, Dictionary<string, SortingTypes> sortExpression) where TEntity : class
        {
            IOrderedQueryable<TEntity> result = null;
            int count = 0;
            foreach (var expression in sortExpression)
            {
                if (expression.Value == SortingTypes.ASC)
                {
                    result = count == 0 ? source.OrderBy(expression.Key) : result.ThenBy(expression.Key);
                }
                else
                {
                    result = count == 0 ? source.OrderByDesc(expression.Key) : result.ThenByDesc(expression.Key);
                }

                count++;
            }
            return result;
        }

        #endregion

        public static IQueryable<TEntity> ToLoad<TEntity>(this IQueryable<TEntity> source, params (Expression<Func<TEntity, Object>> include, Expression<Func<Object, Object>>[] thenInclude)[]
            include) where TEntity : class
        {
            foreach (var expression in include)
            {
                source = source.Include(expression.include);
                if (expression.thenInclude == null) continue;
                source = expression.thenInclude.Aggregate(source,
                    (current, expression1) =>
                        ((IIncludableQueryable<TEntity, object>) current).ThenInclude(expression1));
            }
            return source;
        }
    }
}
