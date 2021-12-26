using PM.Api.DomainEvent.Base;

namespace PM.Api.DomainEvent.Models;

public class ProductDeleted : IDomainEventModel
{
    public string Id { get; set; }
}

