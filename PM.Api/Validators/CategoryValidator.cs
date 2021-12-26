using PM.Api.Exceptions;
using PM.Api.Models;
using PM.Api.Repositories.Implementations;

namespace PM.Api.Validators;

public interface ICategoryValidator
{
    void ValidateId(Models.Category category);
    void ValidateName(Models.Category category);
    void ValidateDescription(Models.Category category);
    void ValidateUpdate(Models.Category category);
    void ValidateCategoryExistance(Models.Product product);
    void ValidateCategoryCanDelete(string categoryId);

}

public class CategoryValidator : ICategoryValidator
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryValidator(IProductRepository productRepository, 
                            ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public void ValidateId(Category category)
    {
        if (string.IsNullOrEmpty(category?.Id))
            throw new Validate.CategoryIdException();
    }

    public void ValidateName(Category category)
    {
        if (string.IsNullOrEmpty(category?.Name))
            throw new Validate.CategoryNameException();
    }

    public void ValidateDescription(Category category)
    {
        if (string.IsNullOrEmpty(category?.Description))
            throw new Validate.CategoryDescriptionException();
    }

    public void ValidateUpdate(Category category)
    {
        if (string.IsNullOrEmpty(category?.Name) && string.IsNullOrEmpty(category?.Description))
            throw new Validate.CategoryCannotUpdate();
    }

    public void ValidateCategoryExistance(Product product)
    {
        if (!_categoryRepository.IsExist(product.Category.Id))
            throw new Validate.CategoryCannotBeFoundException(product.Category.Id);
    }

    public void ValidateCategoryCanDelete(string categoryId)
    {
        if (_productRepository.HasAnyProduct(categoryId))
            throw new Validate.CategoryCannotDeleteException();
    }
}


