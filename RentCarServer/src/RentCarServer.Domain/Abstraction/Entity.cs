namespace RentCarServer.Domain.Abstraction;

public abstract class Entity
{
    protected Entity()
    {
        Id = new IdentityId(Guid.CreateVersion7()); //CreateVersion7 is sortable guid
        IsActive = true;
        CreatedAt = DateTimeOffset.Now;
    }
    public IdentityId Id { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } //datetimeoffset adds gmt+3 to classic datetime
    public IdentityId? CreatedBy { get; private set; } = default!;
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IdentityId? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public IdentityId? DeletedBy { get; private set; }

    public void SetStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void Delete()
    {
        if (IsDeleted) return;
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
    }
}

public sealed record IdentityId(Guid Value) //meaningful, type safe, can add validations, record => immutability
{
    public static implicit operator Guid(IdentityId id) => id.Value; // automatic convertion to Guid and string
    public static implicit operator string(IdentityId id) => id.Value.ToString();
}

