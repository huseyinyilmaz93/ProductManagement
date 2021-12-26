using PM.Api.DomainEvent.Base;

namespace PM.Api.DomainEvent.Models;

public class CategoryUpdated : IDomainEventModel
{
    public string Id { get; set; }
}

