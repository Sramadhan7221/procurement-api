namespace Procurement.Domain.Entities;

public class Product : BaseEntity
{
    public string SKU { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public decimal BasePrice { get; set; }
    public string UoM { get; set; } = null!;
    public string MetaData { get; set; } = string.Empty; 
    public Guid? VendorId {get;set;}
}
