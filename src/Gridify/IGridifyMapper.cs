using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Gridify;

public interface IGridifyMapper<T>
{
   IGridifyMapper<T> GenerateMappings();

   IGridifyMapper<T> AddMap(string from, Expression<Func<T, dynamic?>> destinationExpression, Func<string, object>? convertor = null, bool overrideIfExists = true);
   IGridifyMapper<T> AddMap(string from, Expression<Func<T, int, dynamic?>> destinationExpression, Func<string, object>? convertor = null!,
      bool overrideIfExists = true);
   IGridifyMapper<T> AddMap(IGMap gMap, bool overrideIfExists = true);
   IGridifyMapper<T> AddMap(string from, Func<string, object>? convertor = null!, bool overrideIfExists = true);

   IGridifyMapper<T> RemoveMap(string propertyName);
   IGridifyMapper<T> RemoveMap(IGMap gMap);

   LambdaExpression GetLambdaExpression(string from, StringComparison? comparison = null);
   Expression<Func<T, dynamic>> GetExpression(string key, StringComparison? comparison = null);
   IGMap? GetGMap(string from, StringComparison? comparison = null);
   bool HasMap(string key, StringComparison? comparison = null);
   public GridifyMapperConfiguration Configuration { get; }
   IEnumerable<IGMap> GetCurrentMaps();
   public ParameterExpression? TypeParameter { get; set; }
}

//public interface IGridifyMapper
//{
//   IGridifyMapper<T> AddMap(string from, Expression<Func<T, dynamic?>> destinationExpression, Func<string, object>? convertor = null, bool overrideIfExists = true);

//   IGridifyMapper<T> AddMap(string from, Expression<Func<T, int, dynamic?>> destinationExpression, Func<string, object>? convertor = null!,
//      bool overrideIfExists = true);

//   IGridifyMapper<T> AddMap(IGMap gMap, bool overrideIfExists = true);
//   IGridifyMapper<T> AddMap(string from, Func<string, object>? convertor = null!, bool overrideIfExists = true);
//   IGridifyMapper<T> GenerateMappings();
//   IGridifyMapper<T> RemoveMap(string propertyName);
//   IGridifyMapper<T> RemoveMap(IGMap gMap);
//   LambdaExpression GetLambdaExpression(string from, StringComparison? comparison = null);
//   Expression<Func<T, dynamic>> GetExpression(string key, StringComparison? comparison = null);
//   IGMap? GetGMap(string from, StringComparison? comparison = null);
//   bool HasMap(string key, StringComparison? comparison = null);
//   public GridifyMapperConfiguration Configuration { get; }
//   IEnumerable<IGMap> CurrentMaps();
//   public ParameterExpression? TypeParameter { get; set; }
//}
