namespace Procurement.Application.Features.ProductMaster.Queries.GetProductsFiltered;

public class ProductFilteredDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public string UoM { get; set; } = null!;
    public string? MetaData { get; set; }
    public Guid? VendorId { get; set; }
    public DateTime CreatedAt { get; set; }
}
