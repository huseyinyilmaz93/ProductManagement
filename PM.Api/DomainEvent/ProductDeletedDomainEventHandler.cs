using PM.Api.Constants;
using PM.Api.DomainEvent.Models;
using PM.Api.MemoryCacher;
using PM.Api.Models;

namespace PM.Api.DomainEvent;

public interface IProductDeletedDomainEventHandler
{
    void Handle(ProductDeleted domainEvent);
}

public class ProductDeletedDomainEventHandler : BaseEventHandler<ProductDeleted>, IProductDeletedDomainEventHandler
{
    private readonly IMemoryCacher _memoryCacher;

    public ProductDeletedDomainEventHandler(IMemoryCacher memoryCacher)
    {
        _memoryCacher = memoryCacher;
    }

    public override void Handle(ProductDeleted domainEvent)
    {
        _memoryCacher.DeleteAll(new string[] { $"{PMConstants.ObjectCachePrefix}:{typeof(Product).Name}:{domainEvent.Id}" });
    }
}

