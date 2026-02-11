using ReLoop.Api.Domain.User;
using ReLoop.Application.Abstractions;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Application.Features.Commands.SignUpCommand;

public record SignUpCommand(
    string FirstName,
    string LastName,
    string Email,
    string InputPassword
) : ICommand<Guid>
{
    public sealed class Handler : ICommandHandler<SignUpCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserRepository identityUserRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = identityUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(SignUpCommand command, CancellationToken cancellationToken)
        {
            if (await  _userRepository.ExistsWithEmailAsync(command.Email, cancellationToken))
                return Result<Guid>.BadRequest("Email already exists.");

            var identityUser = User.CreateUser(command.Email, command.FirstName, command.LastName, command.InputPassword);

            await  _userRepository.AddAsync(identityUser, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(identityUser.Id.Value);
        }
    }
}