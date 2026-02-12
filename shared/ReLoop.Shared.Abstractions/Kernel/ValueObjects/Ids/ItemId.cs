using ReLoop.Shared.Abstractions.Kernel.Primitives;

namespace ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;

public record ItemId : EntityId
{
    private ItemId(Guid value) : base(value)
    {
    }

    public static ItemId New() => new(Guid.NewGuid());
    public static ItemId From(Guid value) => new(value);
    public static ItemId From(string value) => new(Guid.Parse(value));

    public static implicit operator Guid(ItemId itemId) => itemId.Value;
    public static implicit operator ItemId(Guid itemId) => new(itemId);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
