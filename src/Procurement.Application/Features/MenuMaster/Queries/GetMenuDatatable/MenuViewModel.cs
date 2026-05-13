namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuDatatable;

public class MenuViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
}
