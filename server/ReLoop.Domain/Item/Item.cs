using ReLoop.Api.Domain.User;
using ReLoop.Shared.Abstractions.Kernel.Primitives;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;

namespace ReLoop.Api.Domain.Item;

public class Item : AggregateRoot<ItemId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public byte[] ImageData { get; private set; }
    public decimal Price { get; private set; }
    public ItemCategory Category { get; private set; }
    public ItemStatus Status { get; private set; }

    public UserId SellerId { get; private set; }
    public User.User Seller { get; private set; }

    public UserId? BuyerId { get; private set; }
    public User.User? Buyer { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? SoldAt { get; private set; }

    protected Item()
    {
    }

    private Item(ItemId id, string name, string description, byte[] imageData, decimal price, ItemCategory category, UserId sellerId) : base(id)
    {
        Name = name;
        Description = description;
        ImageData = imageData;
        Price = price;
        Category = category;
        Status = ItemStatus.Active;
        SellerId = sellerId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Item Create(string name, string description, byte[] imageData, decimal price, ItemCategory category, UserId sellerId)
        => new(ItemId.New(), name, description, imageData, price, category, sellerId);

    public void MarkAsSold(UserId buyerId)
    {
        if (Status == ItemStatus.Sold)
            throw new InvalidOperationException("Item is already sold.");

        if (buyerId == SellerId)
            throw new InvalidOperationException("Seller cannot buy their own item.");

        Status = ItemStatus.Sold;
        BuyerId = buyerId;
        SoldAt = DateTime.UtcNow;
    }
}
