namespace Domain.Entites.Identity;

public sealed class Role : Entity
{
    private Role(
        Guid id,
        string? slug,
        string name,
        string normalizedName) 
        : base(id, slug)
    {
        Name = name;
        NormalizedName = normalizedName;
    }

    public string Name { get; private set; }
    public string NormalizedName { get; private set; }

    public ICollection<UserRole> UserRoles { get; set; }

    public static Role Create(
        string name,
        string? slug = null)
    {
        return new Role(
            Guid.NewGuid(),
            slug,
            name,
            name.ToUpper());
    }
}

