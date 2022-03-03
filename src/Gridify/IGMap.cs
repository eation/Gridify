using System;
using System.Linq.Expressions;

namespace Gridify;

//public interface IGMap
//{
//   string From { get; set; }
//   LambdaExpression DestinationExpression { get; set; }
//   Func<string, object>? Convertor { get; set; }
//}

public interface IGMap
{
   Type Type { get; set; }
   string From { get; set; }
   LambdaExpression DestinationExpression { get; set; }
   Func<string, object>? Convertor { get; set; }
}
