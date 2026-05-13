namespace Procurement.Application.Features.ProductMaster.Queries.GetProductDatatable;

public class ProductDatatableRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string SortDirection { get; set; } = "asc";
}

public class ProductDatatableResponse<T>
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IList<T> Data { get; set; } = new List<T>();
}

public class ProductViewModel
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public string UoM { get; set; } = null!;
    public string? MetaData { get; set; }
    public string? VendorName { get; set; }
    public DateTime CreatedAt { get; set; }
}
