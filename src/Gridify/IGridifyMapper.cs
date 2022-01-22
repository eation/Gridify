using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Gridify;

public interface IGridifyMapper<T>
{
   IGridifyMapper<T> AddMap(string from, Expression<Func<T, dynamic?>> to, Func<string, object>? convertor = null, bool overrideIfExists = true);

   IGridifyMapper<T> AddMap(string from, Expression<Func<T, int, dynamic?>> to, Func<string, object>? convertor = null!,
      bool overrideIfExists = true);

   IGridifyMapper<T> AddMap(IGMap<T> gMap, bool overrideIfExists = true);
   IGridifyMapper<T> AddMap(string from, Func<string, object>? convertor = null!, bool overrideIfExists = true);
   IGridifyMapper<T> GenerateMappings();
   IGridifyMapper<T> RemoveMap(string propertyName);
   IGridifyMapper<T> RemoveMap(IGMap<T> gMap);
   LambdaExpression GetLambdaExpression(string from, StringComparison? comparison = null);
   Expression<Func<T, dynamic>> GetExpression(string key, StringComparison? comparison = null);
   IGMap<T>? GetGMap(string from, StringComparison? comparison = null);
   bool HasMap(string key, StringComparison? comparison = null);
   public GridifyMapperConfiguration Configuration { get; }
   IEnumerable<IGMap<T>> GetCurrentMaps();
}
