using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Auth;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;
using ReLoop.Shared.Contracts.Dto.Identity;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Application.Features.Commands.SignInCommand;

public sealed record SignInCommand(string Email, string Password) : ICommand<AccessToken>
{
    public sealed class Handler : ICommandHandler<SignInCommand, AccessToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public Handler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<AccessToken>> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (user is null || !user.Password.Verify(command.Password))
                return Result<AccessToken>.BadRequest("Invalid credentials");

            var accessToken = AccessToken.Create(
                _jwtProvider.GenerateToken(user.Id.ToString(), user.Email, user.Role.ToString()),
                user.Id,
                user.Email,
                user.Role.ToString());

            return Result<AccessToken>.Ok(accessToken);
        }
    }
}