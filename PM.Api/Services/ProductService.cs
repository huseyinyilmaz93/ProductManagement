using PM.Api.DomainEvent;
using PM.Api.IoC;
using PM.Api.Models;
using PM.Api.Repositories.Implementations;
using PM.Api.Services.Implementations;
using PM.Api.Validators;

namespace PM.Api.Services;

public class ProductService : IProductService
{
    private readonly IValidator _validator;
    private readonly IProductValidator _productValidator;
    private readonly ICategoryValidator _categoryValidator;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    //bool mqttFeatureToggle = false;

    public ProductService(IValidator validator, 
                          IProductValidator productValidator,
                          ICategoryValidator categoryValidator,
                          IProductRepository productRepository, 
                          ICategoryRepository categoryRepository)
    {
        _validator = validator;
        _productValidator = productValidator;
        _categoryValidator = categoryValidator;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public void Create(Product product)
    {
        _productValidator.ValidateName(product);
        _productValidator.ValidateCurrency(product);
        _categoryValidator.ValidateCategoryExistance(product);

        //if(mqttFeatureToggle)
        //    _mqManager.EnsureMqAvailibity();

        string productId = product.Add();
        product.Id = productId;

        //Publish that a product is created.
        //if (mqttFeatureToggle)
        //    _mqManager.Publish()
    }

    public void Delete(string id)
    {
        _validator.ValidateObjectId(id);
        new Product() { Id = id }.Delete();

        //Event raise
        Task.Run(() => IoCHelper.Resolve<IProductDeletedDomainEventHandler>()
            .Handle(new DomainEvent.Models.ProductDeleted() { Id = id }));
    }

    public void Update(Product product)
    {
        _productValidator.ValidateName(product);
        _productValidator.ValidateCurrency(product);
        _categoryValidator.ValidateCategoryExistance(product);

        product.Update();

        //Event raise
        Task.Run(() => IoCHelper.Resolve<IProductUpdatedEventDomainHandler>()
            .Handle(new DomainEvent.Models.ProductUpdated() { Id = product.Id }));
    }
}

