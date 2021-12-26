using PM.Api.DomainEvent.Base;

namespace PM.Api.DomainEvent.Models;

public class ProductUpdated : IDomainEventModel
{
    public string Id { get; set; }
}

