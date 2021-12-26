using PM.Api.Repositories.Base;

namespace PM.Api.Repositories.Implementations;

public interface ICategoryRepository : IRepository<MongoEntities.Category>
{
    void Update(MongoEntities.Category category);

    List<MongoEntities.Category> List(MongoEntities.Category category);
}

