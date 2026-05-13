namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorById;

public class VendorDetailDto
{
    public string Name { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string Address { get; set; } = null!;
}