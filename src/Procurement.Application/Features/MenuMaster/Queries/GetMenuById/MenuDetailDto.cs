namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuById;

public class MenuDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
}
