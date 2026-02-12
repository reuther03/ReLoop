using ReLoop.Api.Domain.Item;
using ReLoop.Shared.Abstractions.Kernel.Database;

namespace ReLoop.Application.Abstractions.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Item>> GetActiveItemsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Item>> GetActiveItemsByCategoryAsync(ItemCategory category, CancellationToken cancellationToken = default);
    Task<IEnumerable<Item>> GetItemsBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
}
