using Microsoft.AspNetCore.Http;
using ReLoop.Api.Domain.Item;
using ReLoop.Application.Abstractions;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;
using ReLoop.Shared.Abstractions.Services;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Application.Features.Commands.CreateItemCommand;

public record CreateItemCommand(
    string Name,
    string Description,
    decimal Price,
    IFormFile Image
) : ICommand<Guid>
{
    public sealed class Handler : ICommandHandler<CreateItemCommand, Guid>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAiService _aiService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IItemRepository itemRepository,
            IAiService aiService,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _itemRepository = itemRepository;
            _aiService = aiService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (!_userService.IsAuthenticated)
                return Result<Guid>.Unauthorized("User not authenticated.");

            if (command.Price <= 0)
                return Result<Guid>.BadRequest("Price must be greater than zero.");

            // Read image bytes
            using var memoryStream = new MemoryStream();
            await command.Image.CopyToAsync(memoryStream, cancellationToken);
            var imageData = memoryStream.ToArray();

            var tagResponse = await _aiService.GenerateTag(
                command.Name,
                command.Description,
                imageData,
                cancellationToken);

            if (!Enum.TryParse<ItemCategory>(tagResponse.Tag, ignoreCase: true, out var category))
                category = ItemCategory.Other;

            var item = Item.Create(
                command.Name,
                command.Description,
                imageData,
                command.Price,
                category,
                _userService.UserId);

            await _itemRepository.AddAsync(item, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(item.Id.Value);
        }
    }
}