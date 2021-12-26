namespace PM.Api.Exceptions;

public class ValidateException : Exception { public ValidateException(string message) : base(message) { } }

public static class Validate
{
    public class ObjectIdException : ValidateException { public ObjectIdException() : base("") { } public ObjectIdException(string id) : base($"Given id({id}) is not valid for ObjectId object") { } }
    public class CategoryIdException : ValidateException { public CategoryIdException() : base("Category.Id cannot be null or empty") { } }
    public class CategoryNameException : ValidateException { public CategoryNameException() : base("Category.Name cannot be null or empty") { } }
    public class CategoryDescriptionException : ValidateException { public CategoryDescriptionException() : base("Category.Description cannot be null or empty") { } }
    public class CategoryCannotUpdate : ValidateException { public CategoryCannotUpdate() : base("At least one field must be given to update any category") { } }
    public class CategoryCannotBeFoundException : ValidateException { public CategoryCannotBeFoundException() : base("") { } public CategoryCannotBeFoundException(string id) : base($"Given category id ({id}) cannot be found") { } }
    public class CategoryCannotDeleteException : ValidateException { public CategoryCannotDeleteException() : base($"Given category cannot be deleted, because it is in use at least one product") { } }
    public class ProductCurrencyException : ValidateException { public ProductCurrencyException() : base($"Product.Currency cannot be null or empty") { } }
    public class ProductNameException : ValidateException { public ProductNameException() : base($"Product.Name cannot be null or empty") { } }
    public class ProductPriceMustBeGreaterThanZeroException : ValidateException { public ProductPriceMustBeGreaterThanZeroException(decimal price) : base($"Given product price ({price}) must be greater than zero") { } }
    public class ProductCannotBeFoundException : ValidateException { public ProductCannotBeFoundException(string id) : base($"Given Product id ({id}) cannot be found") { } }

}

