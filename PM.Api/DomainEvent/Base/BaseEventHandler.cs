using PM.Api.DomainEvent.Base;

namespace PM.Api.DomainEvent;

public abstract class BaseEventHandler<T> where T: IDomainEventModel
{
    public abstract void Handle(T domainEvent);
}

