using MongoDB.Driver;
using PM.Api.Helpers;
using PM.Api.Repositories.Base;
using PM.Api.Repositories.Helpers;
using PM.Api.Repositories.Implementations;
using PM.Api.Repositories.MongoEntities;
using PM.Api.Validators;

namespace PM.Api.Repositories;

public class MongoProductRepository : AbstractMongoRepository<Product>, IProductRepository
{
    public MongoProductRepository(IValidator validator, IConfiguration configuration) : base(validator, configuration)
    { }

    public List<Product> List(Product product)
    {
        var filter = new PMCustomEqualFilter<Product>()
            .Filter(!string.IsNullOrEmpty(product.Name), "Name", product.Name)
            .Filter(!string.IsNullOrEmpty(product.Description), "Description", product.Description)
            .Filter(!string.IsNullOrEmpty(product.Currency), "Currency", product.Currency)
            .Filter(!string.IsNullOrEmpty(product.CategoryId), "CategoryId", product.CategoryId);

        return AsyncHelper.RunSync(async() 
            => await _currentCollection.FindAsync(filter.GetFilter()).ConfigureAwait(false)).ToList();
    }

    public override void Update(Product product)
    {
        _validator.ValidateObjectId(product.Id);

        var builder = Builders<Product>.Update.Set(p => p.Name, product.Name)
            .Set(p => p.Description, product.Description)
            .Set(p => p.Price, product.Price)
            .Set(p => p.CategoryId, product.CategoryId)
            .Set(p => p.Currency, product.Currency);

        AsyncHelper.RunSync(async() => 
            await _currentCollection.FindOneAndUpdateAsync(p => p.Id == product.Id, builder).ConfigureAwait(false));
    }

    public List<string> GetProductIdsByCategoryId(string categoryId)
    {
        _validator.ValidateObjectId(categoryId);
        var filter = new PMCustomEqualFilter<Product>()
                        .Filter(true, "CategoryId", categoryId)
                        .GetFilter();

        return AsyncHelper.RunSync(async() 
            => await _currentCollection.Find(filter).Project(p => p.Id).ToListAsync().ConfigureAwait(false));
    }

    public bool HasAnyProduct(string categoryId)
    {
        return (AsyncHelper.RunSync(async() 
            => await _currentCollection.Find((c) => c.CategoryId == categoryId).Limit(1).CountDocumentsAsync().ConfigureAwait(false))) > default(long);

    }
}

