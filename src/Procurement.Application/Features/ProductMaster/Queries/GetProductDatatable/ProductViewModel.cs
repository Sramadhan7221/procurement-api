namespace Procurement.Application.Features.ProductMaster.Queries.GetProductDatatable;

public class ProductDatatableRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string SortDirection { get; set; } = "asc";
}

public class ProductViewModel
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public string UoM { get; set; } = null!;
    public string? Desc { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? VendorId { get; set; }
    public string? VendorName { get; set; }
    public DateTime CreatedAt { get; set; }
}
