using ReLoop.Application.Abstractions;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;
using ReLoop.Shared.Abstractions.Services;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Application.Features.Commands.SellItemCommand;

public record SellItemCommand(Guid ItemId) : ICommand<Guid>
{
    public sealed class Handler : ICommandHandler<SellItemCommand, Guid>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IItemRepository itemRepository,
            IUserRepository userRepository,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(SellItemCommand command, CancellationToken cancellationToken)
        {
            if (!_userService.IsAuthenticated)
                return Result<Guid>.Unauthorized("User not authenticated.");

            var buyerId = _userService.UserId.Value;

            var item = await _itemRepository.GetByIdAsync(command.ItemId, cancellationToken);
            if (item is null)
                return Result<Guid>.NotFound("Item not found.");

            if (item.Status == Api.Domain.Item.ItemStatus.Sold)
                return Result<Guid>.BadRequest("Item is already sold.");

            var buyer = await _userRepository.GetByIdAsync(buyerId, cancellationToken);
            if (buyer is null)
                return Result<Guid>.NotFound("Buyer not found.");

            if (item.SellerId.Value == buyerId)
                return Result<Guid>.BadRequest("You cannot buy your own item.");

            if (buyer.Balance < item.Price)
                return Result<Guid>.BadRequest("Insufficient balance.");

            var seller = await _userRepository.GetByIdAsync(item.SellerId.Value, cancellationToken);
            if (seller is null)
                return Result<Guid>.NotFound("Seller not found.");

            // Transfer money
            buyer.UpdateBalance(-item.Price);
            seller.UpdateBalance(item.Price);

            // Mark item as sold
            item.MarkAsSold(buyerId);

            _userRepository.Update(buyer);
            _userRepository.Update(seller);
            _itemRepository.Update(item);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(item.Id.Value);
        }
    }
}
