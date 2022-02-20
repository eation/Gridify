namespace Gridify;

public class GridifyQuery : IGridifyQuery
{
   public GridifyQuery()
   {
   }
   public GridifyQuery(int page, int pageSize, string? filter = null, string? orderBy = null, string? select = null)
   {
      Filter = filter;
      OrderBy = orderBy;
      Select = select;

      Page = page;
      PageSize = pageSize;
   }

   public string? Filter { get; set; }
   public string? OrderBy { get; set; }
   public string? Select { get; set; }

   public int Page { get; set; }
   public int PageSize { get; set; }
}
