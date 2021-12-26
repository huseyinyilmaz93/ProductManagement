namespace PM.Api.Repositories.Base;

public interface IRepository<T> where T : IEntity
{
    T Get(string id);
    bool IsExist(string id);
    string Add(T Product);
    void Delete(string id);
}

