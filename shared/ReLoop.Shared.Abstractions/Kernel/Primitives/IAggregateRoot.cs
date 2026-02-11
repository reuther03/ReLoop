using ReLoop.Shared.Abstractions.Kernel.Events;

namespace ReLoop.Shared.Abstractions.Kernel.Primitives;

public interface IAggregateRoot
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void RaiseDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}