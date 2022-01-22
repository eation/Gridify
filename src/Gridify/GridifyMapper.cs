#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gridify;

public class GridifyMapper<T> : IGridifyMapper<T>
{
   public GridifyMapperConfiguration Configuration { get; }
   private readonly List<IGMap<T>> _mappings;
   /// <summary>
   /// Default Type ParameterExpression,It's named 'm'
   /// </summary>
   public ParameterExpression? TypeParameter { get; set; }

   public GridifyMapper(ParameterExpression? parameter = null, bool autoGenerateMappings = false)
   {
      Configuration = new GridifyMapperConfiguration();
      _mappings = new List<IGMap<T>>();
      TypeParameter = parameter ?? Expression.Parameter(typeof(T), "m");

      if (autoGenerateMappings)
         GenerateMappings();
   }

   public GridifyMapper(GridifyMapperConfiguration configuration, ParameterExpression? parameter = null, bool autoGenerateMappings = false)
   {
      Configuration = configuration;
      _mappings = new List<IGMap<T>>();
      TypeParameter = parameter ?? Expression.Parameter(typeof(T), "m");

      if (autoGenerateMappings)
         GenerateMappings();
   }

   public GridifyMapper(Action<GridifyMapperConfiguration> configuration, bool autoGenerateMappings = false)
   {
      Configuration = new GridifyMapperConfiguration();
      configuration.Invoke(Configuration);
      _mappings = new List<IGMap<T>>();
      TypeParameter = Expression.Parameter(typeof(T), "m");

      if (autoGenerateMappings)
         GenerateMappings();
   }

   public IGridifyMapper<T> AddMap(string from, Func<string, object>? convertor = null!, bool overrideIfExists = true)
   {
      if (!overrideIfExists && HasMap(from))
         throw new GridifyMapperException($"Duplicate Key. the '{from}' key already exists");

      //Expression<Func<T, dynamic>> to;
      LambdaExpression to;
      try
      {
         to = GridifyMapperHelper.CreateExpression<T>(TypeParameter!, from);
      }
      catch (Exception)
      {
         throw new GridifyMapperException($"Property '{from}' not found.");
      }

      RemoveMap(from);
      _mappings.Add(new GMap<T>(from, to!, convertor));
      return this;
   }

   private IList<GMap<T>> GenerateRefMappings(Expression refExpression, string refPropertyName, Type type, Type refType)
   {
      try
      {
         var mappings = new List<GMap<T>>();
         var properties = type.GetProperties();
         foreach (var item in properties)
         {
            var name = char.ToLowerInvariant(refPropertyName[0]) + refPropertyName.Substring(1) + "." + char.ToLowerInvariant(item.Name[0]) + item.Name.Substring(1); // camel-case name

            // skip List
            if (item.PropertyType != refType && ((item.PropertyType == typeof(string) || (item.PropertyType.IsValueType && !item.PropertyType.GetInterfaces().Any(m => m.Name == typeof(IEnumerable<>).Name)))))
            {
               if (refExpression is LambdaExpression lambda)
               {
                  //if (lambda.Body.NodeType== ExpressionType.Convert||lambda.Body.NodeType== ExpressionType.ConvertChecked )
                  if (lambda.Body is UnaryExpression unary)
                  {
                     var subExpression0 = GridifyMapperHelper.CreateSubExpression<T>(TypeParameter!, unary.Operand, item.Name)!;
                     mappings.Add(new GMap<T>(name, subExpression0!));
                  }
                  else if (lambda.Body is MemberExpression member)
                  {
                     var subExpression1 = GridifyMapperHelper.CreateSubExpression<T>(TypeParameter!, member, item.Name)!;
                     mappings.Add(new GMap<T>(name, subExpression1!));
                  }
               }
               else
               {
                  var subExpression2 = GridifyMapperHelper.CreateSubExpression<T>(TypeParameter!, refExpression, item.Name)!;
                  mappings.Add(new GMap<T>(name, subExpression2!));
               }
            }
            else if (item.PropertyType != refType && (item.PropertyType.IsClass && item.PropertyType != typeof(string) && !item.PropertyType.IsValueType && !item.PropertyType.GetInterfaces().Any(m => m.Name == typeof(IEnumerable<>).Name)))
            {
               mappings.AddRange(GenerateRefMappings(refExpression, name, item.PropertyType, item.DeclaringType!));
            }

         }

         return mappings;
      }
      catch (Exception)
      {
         throw;
      }
   }

   public IGridifyMapper<T> GenerateMappings()
   {
      try
      {
         var properties = typeof(T).GetProperties();
         foreach (var item in properties)
         {
            var name = char.ToLowerInvariant(item.Name[0]) + item.Name.Substring(1); // camel-case name
            var expression = GridifyMapperHelper.CreateExpression<T>(TypeParameter!, item.Name)!;
            // skip List

            if (item.PropertyType == typeof(string) || (item.PropertyType.IsValueType && !item.PropertyType.GetInterfaces().Any(m => m.Name == typeof(IEnumerable<>).Name)))
            {
               _mappings.Add(new GMap<T>(name, expression!));
            }
            else if (item.PropertyType.IsClass && item.PropertyType != typeof(string) && !item.PropertyType.IsValueType && !item.PropertyType.GetInterfaces().Any(m => m.Name == typeof(IEnumerable<>).Name))
            {
               _mappings.AddRange(GenerateRefMappings(expression, name, item.PropertyType, item.DeclaringType!));
            }
         }
         return this;
      }
      catch (Exception)
      {
         throw;
      }
   }

   public IGridifyMapper<T> AddMap(string from, Expression<Func<T, dynamic?>> to, Func<string, object>? convertor = null!,
      bool overrideIfExists = true)
   {
      if (!overrideIfExists && HasMap(from))
         throw new GridifyMapperException($"Duplicate Key. the '{from}' key already exists");

      RemoveMap(from);
      _mappings.Add(new GMap<T>(from, to, convertor));
      return this;
   }

   public IGridifyMapper<T> AddMap(string from, Expression<Func<T, int, dynamic?>> to, Func<string, object>? convertor = null!,
      bool overrideIfExists = true)
   {
      if (!overrideIfExists && HasMap(from))
         throw new GridifyMapperException($"Duplicate Key. the '{from}' key already exists");

      RemoveMap(from);
      _mappings.Add(new GMap<T>(from, to, convertor));
      return this;
   }

   public IGridifyMapper<T> AddMap(IGMap<T> gMap, bool overrideIfExists = true)
   {
      if (!overrideIfExists && HasMap(gMap.From))
         throw new GridifyMapperException($"Duplicate Key. the '{gMap.From}' key already exists");

      RemoveMap(gMap.From);
      _mappings.Add(gMap);
      return this;
   }

   public IGridifyMapper<T> RemoveMap(string from)
   {
      var map = GetGMap(from);
      if (map != null)
         _mappings.Remove(map);
      return this;
   }

   public IGridifyMapper<T> RemoveMap(IGMap<T> gMap)
   {
      _mappings.Remove(gMap);
      return this;
   }

   public bool HasMap(string from, StringComparison? comparison = null)
   {
      if (comparison != null)
      {
         return _mappings.Any(q => from.Equals(q.From, comparison.Value));
      }
      else
      {
         return Configuration.CaseSensitive
                  ? _mappings.Any(q => from.Equals(q.From))
                  : _mappings.Any(q => from.Equals(q.From, StringComparison.InvariantCultureIgnoreCase));
      }
   }

   public IGMap<T>? GetGMap(string from, StringComparison? comparison = null)
   {
      if (comparison != null)
      {
         return _mappings.FirstOrDefault(q => from.Equals(q.From, comparison.Value));
      }
      else
      {
         return Configuration.CaseSensitive
            ? _mappings.FirstOrDefault(q => from.Equals(q.From))
            : _mappings.FirstOrDefault(q => from.Equals(q.From, StringComparison.InvariantCultureIgnoreCase));
      }
   }

   public LambdaExpression GetLambdaExpression(string key, StringComparison? comparison = null)
   {
      LambdaExpression? expression = null;
      if (comparison != null)
      {
         expression = _mappings.FirstOrDefault(q => key.Equals(q.From, comparison.Value))?.To;
      }
      else
      {
         expression = Configuration.CaseSensitive
                  ? _mappings.FirstOrDefault(q => key.Equals(q.From))?.To
                  : _mappings.FirstOrDefault(q => key.Equals(q.From, StringComparison.InvariantCultureIgnoreCase))?.To;
      }

      if (expression == null)
         throw new GridifyMapperException($"Mapping Key `{key}` not found.");
      return expression!;
   }

   public Expression<Func<T, dynamic>> GetExpression(string key, StringComparison? comparison = null)
   {
      LambdaExpression? expression = null;
      if (comparison != null)
      {
         expression = _mappings.FirstOrDefault(q => key.Equals(q.From, comparison.Value))?.To;
      }
      else
      {
         expression = Configuration.CaseSensitive
                 ? _mappings.FirstOrDefault(q => key.Equals(q.From))?.To
                 : _mappings.FirstOrDefault(q => key.Equals(q.From, StringComparison.InvariantCultureIgnoreCase))?.To;
      }

      if (expression == null)
         throw new GridifyMapperException($"Mapping Key `{key}` not found.");
      return expression as Expression<Func<T, dynamic>> ?? throw new GridifyMapperException($"Expression fir the `{key}` not found.");
   }

   public IEnumerable<IGMap<T>> GetCurrentMaps()
   {
      return _mappings;
   }

   /// <summary>
   /// Converts current mappings to a comma seperated list of map names.
   /// eg, field1,field2,field3
   /// </summary>
   /// <returns>a comma seperated string</returns>
   public override string ToString() => string.Join(",", _mappings.Select(q => q.From));

}
