using PM.Api.Repositories.Base;

namespace PM.Api.Repositories.Implementations;

public interface IProductRepository : IRepository<MongoEntities.Product>
{
    List<string> GetProductIdsByCategoryId(string categoryId);

    void Update(MongoEntities.Product Product);

    List<MongoEntities.Product> List(MongoEntities.Product product);

    bool HasAnyProduct(string categoryId);
}

