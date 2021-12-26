using PM.Api.Exceptions;
using PM.Api.IoC;
using PM.Api.Repositories.Implementations;

namespace PM.Api.Models;

public class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    private readonly ICategoryRepository _categoryRepository;

    public Category()
    {
        _categoryRepository = IoCHelper.Resolve<ICategoryRepository>();
    }

    public string Add()
    {
        return _categoryRepository.Add(new Repositories.MongoEntities.Category() 
        { 
            Description = Description, 
            Name = Name 
        });
    }

    public void Update()
    {
        Repositories.MongoEntities.Category categoryDocument = _categoryRepository.Get(Id);

        if(categoryDocument == null)
            throw new Validate.CategoryCannotBeFoundException(Id);

        categoryDocument.Name = Name;
        categoryDocument.Description = Description;
        
        _categoryRepository.Update(categoryDocument);
    }

    public void Delete()
    {
        if(!_categoryRepository.IsExist(Id))
            throw new Validate.CategoryCannotBeFoundException(Id);

        _categoryRepository.Delete(Id);
    }
}

