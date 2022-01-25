using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Gridify;

public class GMap<T> : IGMap<T>
{
   public string From { get; set; }
   public LambdaExpression DestinationExpression { get; set; }
   public Func<string, object>? Convertor { get; set; }

   public GMap(string from, LambdaExpression destinationExpression, Func<string, object>? convertor = null)
   {
      From = from;
      DestinationExpression = destinationExpression;
      Convertor = convertor;
   }

   internal bool IsNestedCollection() => Regex.IsMatch(DestinationExpression.ToString(), @"\.Select\s*\(");

   //public GMap(string from, Expression<Func<T, int, dynamic?>> destinationExpression, Func<string, object>? convertor = null)
   //{
   //   From = from;
   //   DestinationExpression = destinationExpression;
   //   Convertor = convertor;
   //}
}
