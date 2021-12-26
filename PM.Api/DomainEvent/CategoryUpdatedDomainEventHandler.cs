using PM.Api.Constants;
using PM.Api.DomainEvent.Models;
using PM.Api.MemoryCacher;
using PM.Api.Models;
using PM.Api.Repositories.Implementations;

namespace PM.Api.DomainEvent;

public interface ICategoryUpdatedDomainEventHandler
{
    void Handle(CategoryUpdated domainEvent);
}

public class CategoryUpdatedDomainEventHandler : BaseEventHandler<CategoryUpdated>, ICategoryUpdatedDomainEventHandler
{
    private readonly IMemoryCacher _memoryCacher;
    private readonly IProductRepository _productRepository;

    public CategoryUpdatedDomainEventHandler(IMemoryCacher memoryCacher, IProductRepository productRepository)
    {
        _memoryCacher = memoryCacher;
        _productRepository = productRepository;
    }

    public override void Handle(CategoryUpdated domainEvent)
    {
        List<string> productIds = _productRepository.GetProductIdsByCategoryId(domainEvent.Id);
        _memoryCacher.DeleteAll(productIds.Select(pId => $"{PMConstants.ObjectCachePrefix}:{typeof(Product).Name}:{pId}").ToArray());
    }
}

