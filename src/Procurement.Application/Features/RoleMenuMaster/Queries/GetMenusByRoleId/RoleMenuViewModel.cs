namespace Procurement.Application.Features.RoleMenuMaster.Queries.GetMenusByRoleId;

public class RoleMenuViewModel
{
    public Guid MenuId { get; set; }
    public string Name { get; set; } = null!;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
}
