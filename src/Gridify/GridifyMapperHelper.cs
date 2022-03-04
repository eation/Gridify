using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gridify
{
   public static class GridifyMapperHelper
   {
      public static Expression<Func<T, dynamic>> CreateExpression<T>(ParameterExpression TypeParameter, string from)
      {
         // x =>
         //var parameter = Expression.Parameter(typeof(T));
         // x.Name,x.yyy.zz.xx
         Expression mapProperty = TypeParameter!;
         mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
         // (object)x.Name
         var convertedExpression = Expression.Convert(mapProperty, typeof(object));
         // x => (object)x.Name
         return Expression.Lambda<Func<T, object>>(convertedExpression, TypeParameter!);
      }

      public static Expression<Func<T, dynamic>> CreateSubExpression<T>(ParameterExpression TypeParameter, Expression root, string from)
      {
         if (root is LambdaExpression lambda)
         {
            // x =>
            //var parameter = Expression.Parameter(typeof(T));
            // x.Name,x.yyy.zz.xx
            Expression mapProperty = lambda.Body;
            mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
            // (object)x.Name
            var convertedExpression = Expression.Convert(mapProperty, typeof(object));
            // x => (object)x.Name
            return Expression.Lambda<Func<T, object>>(convertedExpression, TypeParameter);
         }
         else
         {
            // x =>
            //var parameter = Expression.Parameter(typeof(T));
            // x.Name,x.yyy.zz.xx
            Expression mapProperty = root;
            mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
            // (object)x.Name
            var convertedExpression = Expression.Convert(mapProperty, typeof(object));
            // x => (object)x.Name
            return Expression.Lambda<Func<T, object>>(convertedExpression, TypeParameter!);
         }
      }

      public static LambdaExpression CreateLambdaExpression<T>(ParameterExpression TypeParameter, string from)
      {
         // x =>
         //var parameter = Expression.Parameter(typeof(T));
         // x.Name,x.yyy.zz.xx
         Expression mapProperty = TypeParameter!;
         mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
         // (object)x.Name
         var convertedExpression = Expression.Convert(mapProperty, typeof(object));
         // x => (object)x.Name
         return Expression.Lambda(convertedExpression, TypeParameter!);
      }

      public static LambdaExpression CreateSubLambdaExpression<T>(ParameterExpression TypeParameter, Expression root, string from)
      {
         if (root is LambdaExpression lambda)
         {
            // x =>
            //var parameter = lambda.Parameters;
            // x.Name,x.yyy.zz.xx
            Expression mapProperty = lambda.Body;
            mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
            // (object)x.Name
            //var convertedExpression = Expression.Convert(mapProperty, typeof(object));
            // x => (object)x.Name
            //return Expression.Lambda(convertedExpression, parameter);
            return Expression.Lambda(mapProperty, TypeParameter);
         }
         else
         {
            //var parameter = Expression.Parameter(typeof(T));
            // x.Name,x.yyy.zz.xx
            Expression mapProperty = root;
            mapProperty = from.Split('.').Aggregate<string, Expression>(mapProperty, Expression.Property);
            // (object)x.Name
            //var convertedExpression = Expression.Convert(mapProperty, typeof(object));
            // x => (object)x.Name
            //return Expression.Lambda(convertedExpression, parameter);
            return Expression.Lambda(mapProperty, TypeParameter!);
         }
      }


      //public static Expression<Func<T, dynamic>> CreateExpression<T>(this IGridifyMapper<T> mapper, string from)
      //{
      //   // x =>
      //   //var parameter = Expression.Parameter(typeof(T));
      //   // x.Name,x.yyy.zz.xx
      //   Expression mapProperty = mapper.TypeParameter!;
      //   foreach (var propertyName in from.Split('.'))
      //   {
      //      mapProperty = Expression.Property(mapProperty, propertyName);
      //   }
      //   // (object)x.Name
      //   var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //   // x => (object)x.Name
      //   return Expression.Lambda<Func<T, object>>(convertedExpression, mapper.TypeParameter!);
      //}

      //public static Expression<Func<T, dynamic>> CreateSubExpression<T>(this IGridifyMapper<T> mapper, Expression root, string from)
      //{
      //   if (root is LambdaExpression lambda)
      //   {
      //      // x =>
      //      //var parameter = Expression.Parameter(typeof(T));
      //      // x.Name,x.yyy.zz.xx
      //      Expression mapProperty = lambda.Body;
      //      foreach (var propertyName in from.Split('.'))
      //      {
      //         mapProperty = Expression.Property(mapProperty, propertyName);
      //      }
      //      // (object)x.Name
      //      var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //      // x => (object)x.Name
      //      return Expression.Lambda<Func<T, object>>(convertedExpression, lambda.Parameters);
      //   }
      //   else
      //   {
      //      // x =>
      //      //var parameter = Expression.Parameter(typeof(T));
      //      // x.Name,x.yyy.zz.xx
      //      Expression mapProperty = root;
      //      foreach (var propertyName in from.Split('.'))
      //      {
      //         mapProperty = Expression.Property(mapProperty, propertyName);
      //      }
      //      // (object)x.Name
      //      var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //      // x => (object)x.Name
      //      return Expression.Lambda<Func<T, object>>(convertedExpression, mapper.TypeParameter!);
      //   }
      //}

      //public static LambdaExpression CreateLambdaExpression<T>(this IGridifyMapper<T> mapper, string from)
      //{
      //   // x =>
      //   //var parameter = Expression.Parameter(typeof(T));
      //   // x.Name,x.yyy.zz.xx
      //   Expression mapProperty = mapper.TypeParameter!;
      //   foreach (var propertyName in from.Split('.'))
      //   {
      //      mapProperty = Expression.Property(mapProperty, propertyName);
      //   }
      //   // (object)x.Name
      //   var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //   // x => (object)x.Name
      //   return Expression.Lambda(convertedExpression, mapper.TypeParameter!);
      //}

      //public static LambdaExpression CreateSubLambdaExpression<T>(this IGridifyMapper<T> mapper, Expression root, string from)
      //{
      //   if (root is LambdaExpression lambda)
      //   {
      //      // x =>
      //      //var parameter = lambda.Parameters;
      //      // x.Name,x.yyy.zz.xx
      //      Expression mapProperty = lambda.Body;
      //      foreach (var propertyName in from.Split('.'))
      //      {
      //         mapProperty = Expression.Property(mapProperty, propertyName);
      //      }
      //      // (object)x.Name
      //      //var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //      // x => (object)x.Name
      //      //return Expression.Lambda(convertedExpression, parameter);
      //      return Expression.Lambda(mapProperty, lambda.Parameters);
      //   }
      //   else
      //   {
      //      //var parameter = Expression.Parameter(typeof(T));
      //      // x.Name,x.yyy.zz.xx
      //      Expression mapProperty = root;
      //      foreach (var propertyName in from.Split('.'))
      //      {
      //         mapProperty = Expression.Property(mapProperty, propertyName);
      //      }
      //      // (object)x.Name
      //      //var convertedExpression = Expression.Convert(mapProperty, typeof(object));
      //      // x => (object)x.Name
      //      //return Expression.Lambda(convertedExpression, parameter);
      //      return Expression.Lambda(mapProperty, mapper.TypeParameter!);
      //   }
      //}

   }
}
