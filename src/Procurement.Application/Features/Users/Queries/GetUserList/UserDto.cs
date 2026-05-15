namespace Procurement.Application.Features.Users.Queries.GetUserList;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid Role { get; set; }
    public string RoleName { get; set; } = null!;
}