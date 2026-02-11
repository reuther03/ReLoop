using ReLoop.Shared.Abstractions.QueriesAndCommands.Notifications;

namespace ReLoop.Shared.Abstractions.Kernel.Events;

public interface IDomainNotificationHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;