namespace Procurement.Domain.Entities;

public class RoleMenu : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public Guid MenuId { get; set; }
    public Menu Menu { get; set; } = null!;
}
