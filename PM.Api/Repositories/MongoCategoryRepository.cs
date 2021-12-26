using MongoDB.Driver;
using PM.Api.Helpers;
using PM.Api.Repositories.Base;
using PM.Api.Repositories.Helpers;
using PM.Api.Repositories.Implementations;
using PM.Api.Repositories.MongoEntities;
using PM.Api.Validators;

namespace PM.Api.Repositories;

public class MongoCategoryRepository : AbstractMongoRepository<Category>, ICategoryRepository
{
    public MongoCategoryRepository(IValidator validator, IConfiguration configuration) : base(validator, configuration)
    { }

    public List<Category> List(Category category)
    {
        var filter = new PMCustomEqualFilter<Category>()
            .Filter(!string.IsNullOrEmpty(category.Name), "Name", category.Name)
            .Filter(!string.IsNullOrEmpty(category.Description), "Description", category.Description);
        return AsyncHelper.RunSync(async() 
            => await (_currentCollection.FindAsync(filter.GetFilter()).ConfigureAwait(false))).ToList();
    }

    public override void Update(Category category)
    {
        _validator.ValidateObjectId(category.Id);

        AsyncHelper.RunSync(async() => await _currentCollection.FindOneAndUpdateAsync(c => c.Id == category.Id,
                    Builders<Category>.Update.Set(c => c.Name, category.Name)
                                             .Set(c => c.Description, category.Description)).ConfigureAwait(false));
    }

}

