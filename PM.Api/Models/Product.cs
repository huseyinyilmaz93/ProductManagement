using PM.Api.Exceptions;
using PM.Api.IoC;
using PM.Api.Repositories.Implementations;

namespace PM.Api.Models;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }

    private readonly IProductRepository _productRepository;

    public Product()
    {
        _productRepository = IoCHelper.Resolve<IProductRepository>();
    }

    public string Add()
    {
        return _productRepository.Add(new Repositories.MongoEntities.Product()
        { 
            CategoryId = Category.Id,
            Name = Name,
            Description = Description,
            Price = Price,
            Currency = Currency
        });
    }

    public void Update()
    {
        Repositories.MongoEntities.Product productDocument = _productRepository.Get(Id);

        if (productDocument == null)
            throw new Validate.ProductCannotBeFoundException(Id);

        productDocument.Currency = Currency;
        productDocument.Description = Description;
        productDocument.Price = Price;
        productDocument.CategoryId = Category.Id;
        productDocument.Name = Name;

        _productRepository.Update(productDocument);
    }

    public void Delete()
    {
        if (!_productRepository.IsExist(Id))
            throw new Validate.ProductCannotBeFoundException(Id);

        _productRepository.Delete(Id);
    }


}

