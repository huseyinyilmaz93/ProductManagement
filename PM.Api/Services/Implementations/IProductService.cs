namespace PM.Api.Services.Implementations;

public interface IProductService
{
    public void Create(Models.Product Product);
    public void Update(Models.Product Product);
    public void Delete(string id);
}

