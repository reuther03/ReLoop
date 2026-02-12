using Microsoft.EntityFrameworkCore;
using ReLoop.Api.Domain.Item;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;
using ReLoop.Shared.Infrastructure.Postgres;

namespace ReLoop.Infrastructure.Database.Repository;

internal class ItemRepository : Repository<Item, ReLoopDbContext>, IItemRepository
{
    private readonly ReLoopDbContext _context;

    public ItemRepository(ReLoopDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Items
            .Include(i => i.Seller)
            .Include(i => i.Buyer)
            .FirstOrDefaultAsync(i => i.Id == ItemId.From(id), cancellationToken);

    public async Task<IEnumerable<Item>> GetActiveItemsAsync(CancellationToken cancellationToken = default)
        => await _context.Items
            .Include(i => i.Seller)
            .Where(i => i.Status == ItemStatus.Active)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Item>> GetActiveItemsExcludingSellerAsync(Guid excludeSellerId, CancellationToken cancellationToken = default)
        => await _context.Items
            .Include(i => i.Seller)
            .Where(i => i.Status == ItemStatus.Active && i.SellerId != UserId.From(excludeSellerId))
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Item>> GetActiveItemsByCategoryAsync(ItemCategory category, CancellationToken cancellationToken = default)
        => await _context.Items
            .Include(i => i.Seller)
            .Where(i => i.Status == ItemStatus.Active && i.Category == category)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Item>> GetItemsBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
        => await _context.Items
            .Include(i => i.Buyer)
            .Where(i => i.SellerId == UserId.From(sellerId))
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
}