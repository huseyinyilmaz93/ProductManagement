using PM.Api.Exceptions;
using PM.Api.Models;

namespace PM.Api.Validators;

public interface IProductValidator
{
    void ValidateName(Models.Product product);
    void ValidateCurrency(Models.Product product);
    void ValidatePrice(Models.Product product);
}

public class ProductValidator : IProductValidator
{
    public void ValidateCurrency(Product product)
    {
        if (string.IsNullOrEmpty(product.Currency))
            throw new Validate.ProductCurrencyException();
    }

    public void ValidateName(Product product)
    {
        if (string.IsNullOrEmpty(product.Name))
            throw new Validate.ProductNameException();
    }

    public void ValidatePrice(Product product)
    {
        if (string.IsNullOrEmpty(product.Currency))
            throw new Validate.ProductPriceMustBeGreaterThanZeroException(product.Price);
    }
}


