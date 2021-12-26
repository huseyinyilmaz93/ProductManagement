using MongoDB.Driver;
using PM.Api.Constants;
using PM.Api.Helpers;
using PM.Api.Validators;

namespace PM.Api.Repositories.Base;

//TODO: Retry and circuit breaker implementation with Polly.
public abstract class AbstractMongoRepository<T> where T : IEntity
{
    private string _collectionName;

    protected IValidator _validator;

    protected readonly IMongoCollection<T> _currentCollection;

    public AbstractMongoRepository(IValidator validator, IConfiguration configuration)
    {
        _collectionName = typeof(T).Name;
        _validator = validator;
        string connectionString = configuration.GetValue<string>(PMConstants.MongoDbConfigurationAccessor);
        var client = new MongoClient(connectionString);
        IMongoDatabase db = client.GetDatabase(PMConstants.DefaultDatabase);
        var collectionList = db.ListCollectionNames().ToList();
        if (!collectionList.Contains(_collectionName))
            db.CreateCollection(_collectionName);

        _currentCollection = db.GetCollection<T>(_collectionName);
    }

    public T Get(string id)
    {
        _validator.ValidateObjectId(id);
        return AsyncHelper.RunSync(async() 
            => await _currentCollection.FindAsync(c => c.Id == id).ConfigureAwait(false)).FirstOrDefault();
    }

    public bool IsExist(string id)
    {
        _validator.ValidateObjectId(id);
        return (AsyncHelper.RunSync(async() 
            => await _currentCollection.Find((c) => c.Id == id).Limit(1).CountDocumentsAsync().ConfigureAwait(false)) > default(long));
    }

    public string Add(T document)
    {
        AsyncHelper.RunSync(async() 
            => await _currentCollection.InsertOneAsync(document).ConfigureAwait(false));
        return document.Id.ToString();
    }

    public void Delete(string id)
    {
        _validator.ValidateObjectId(id);
        AsyncHelper.RunSync(async() 
            => await _currentCollection.FindOneAndDeleteAsync((c) => c.Id == id).ConfigureAwait(false));
    }

    public abstract void Update(T document);
}

