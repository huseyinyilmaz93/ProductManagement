namespace PM.Api.Services.Implementations;

public interface ICategoryService
{
    public void Create(Models.Category category);
    public void Revise(Models.Category category);
    public void Delete(string id);
        
}

