using PM.Api.Constants;
using PM.Api.DomainEvent.Models;
using PM.Api.MemoryCacher;
using PM.Api.Models;

namespace PM.Api.DomainEvent;

public interface IProductUpdatedEventDomainHandler
{
    void Handle(ProductUpdated domainEvent);
}

public class ProductUpdatedEventDomainHandler : BaseEventHandler<ProductUpdated>, IProductUpdatedEventDomainHandler
{
    private readonly IMemoryCacher _memoryCacher;

    public ProductUpdatedEventDomainHandler(IMemoryCacher memoryCacher)
    {
        _memoryCacher = memoryCacher;
    }

    public override void Handle(ProductUpdated domainEvent)
    {
        _memoryCacher.DeleteAll(new string[] { $"{PMConstants.ObjectCachePrefix}:{typeof(Product).Name}:{domainEvent.Id}" });
    }
}

