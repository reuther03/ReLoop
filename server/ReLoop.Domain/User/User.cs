using ReLoop.Shared.Abstractions.Kernel.Primitives;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;

namespace ReLoop.Api.Domain.User;

public class User : AggregateRoot<UserId>
{
    public Email Email { get; private set; }
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Password Password { get; private set; }
    public Role Role { get; private set; }
    public decimal Balance { get; private set; }

    protected User()
    {
    }

    private User(UserId id, Email email, Name firstName, Name lastName, Password password, Role role) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        Balance = 500; // Initial balance for new users
    }

    public static User CreateUser(Email email, Name firstName, Name lastName, Password password)
        => new(Guid.NewGuid(), email, firstName, lastName, password, Role.User);


    public static User CreateAdmin(Email email, Name firstName, Name lastName, Password password) =>
        new(Guid.NewGuid(), email, firstName, lastName, password, Role.Admin);

    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }
}