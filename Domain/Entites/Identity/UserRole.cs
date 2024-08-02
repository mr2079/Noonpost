namespace Domain.Entites.Identity;

public sealed class UserRole
{
    private UserRole(
        Guid userId,
        Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }

    public User User { get; set; }
    public Role Role { get; set; }

    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole(userId, roleId);
    }
}