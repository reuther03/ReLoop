using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Application.Features.Dtos;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Queries;
using ReLoop.Shared.Abstractions.Services;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Application.Features.Queries.GetItemsQuery;

public record GetItemsQuery : IQuery<IEnumerable<ItemDto>>
{
    public sealed class Handler : IQueryHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserService _userService;

        public Handler(IItemRepository itemRepository, IUserService userService)
        {
            _itemRepository = itemRepository;
            _userService = userService;
        }

        public async Task<Result<IEnumerable<ItemDto>>> Handle(GetItemsQuery query, CancellationToken cancellationToken = default)
        {
            var items = _userService.IsAuthenticated
                ? await _itemRepository.GetActiveItemsExcludingSellerAsync(_userService.UserId.Value, cancellationToken)
                : await _itemRepository.GetActiveItemsAsync(cancellationToken);

            var itemDtos = items.Select(i => new ItemDto(
                i.Id.Value,
                i.Name,
                i.Description,
                i.ImageData,
                i.Price,
                i.Category.ToString(),
                i.Status.ToString(),
                i.SellerId.Value,
                $"{i.Seller.FirstName} {i.Seller.LastName}",
                i.CreatedAt
            ));

            return Result.Ok(itemDtos);
        }
    }
}
