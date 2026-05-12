namespace Procurement.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }

    public Role Role { get; set; } = null!;
    public ICollection<ProcurementRequest> ProcurementRequests { get; set; } = new List<ProcurementRequest>();
}
