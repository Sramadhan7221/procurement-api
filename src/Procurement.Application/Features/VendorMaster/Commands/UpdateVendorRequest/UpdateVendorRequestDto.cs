namespace Procurement.Application.Features.VendorMaster.Commands.UpdateVendorRequest;
public class UpdateVendorRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string Address { get; set; } = null!;
}