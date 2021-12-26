using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.Api.Repositories;
using PM.Api.Repositories.Implementations;
using PM.Api.Test.Base;
using System.Threading.Tasks;

namespace PM.Api.Test.RepositoryTests;

[TestClass]
public class CategoryRepositoryTest : BaseUnitTest
{
    ICategoryRepository _categoryRepository;
    public CategoryRepositoryTest()
    {
        _categoryRepository = new MongoCategoryRepository(validatorMock.Object, configuration);
    }

    //TODO: repo unit test.
}
