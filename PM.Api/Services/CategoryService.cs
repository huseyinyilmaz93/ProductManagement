using PM.Api.DomainEvent;
using PM.Api.IoC;
using PM.Api.Models;
using PM.Api.Services.Implementations;
using PM.Api.Validators;

namespace PM.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly IValidator _validator;
    private readonly IProductValidator _productValidator;
    private readonly ICategoryValidator _categoryValidator;

    //bool mqttFeatureToggle = false;

    public CategoryService(IValidator validator, 
        IProductValidator productValidator, 
        ICategoryValidator categoryValidator)
    {
        _validator = validator;
        _productValidator = productValidator;
        _categoryValidator = categoryValidator;
    }

    public void Create(Category category)
    {
        _categoryValidator.ValidateName(category);
        _categoryValidator.ValidateDescription(category);

        //if(mqttFeatureToggle)
        //    _mqManager.EnsureMqAvailibity();

        string categoryId = category.Add();
        category.Id = categoryId;

        //Publish that a category is created.
        //if (mqttFeatureToggle)
        //    _mqManager.Publish()
    }

    public void Delete(string id)
    {
        _validator.ValidateObjectId(id);
        _categoryValidator.ValidateCategoryCanDelete(id);
        new Category() { Id = id }.Delete();
    }

    public void Revise(Category category)
    {
        _validator.ValidateObjectId(category.Id);
        _categoryValidator.ValidateUpdate(category);
        _categoryValidator.ValidateName(category);

        category.Update();

        //Raise event
        Task.Run(() => IoCHelper.Resolve<ICategoryUpdatedDomainEventHandler>()
            .Handle(new DomainEvent.Models.CategoryUpdated() { Id = category.Id }));
    }
}

