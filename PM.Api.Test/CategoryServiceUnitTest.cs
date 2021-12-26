using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;
using PM.Api.Exceptions;
using PM.Api.Services;
using PM.Api.Services.Implementations;
using PM.Api.Test.Base;
using System.Threading.Tasks;

namespace PM.Api.Test
{
    [TestClass]
    public class CategoryServiceUnitTest : BaseUnitTest
    {
        ICategoryService _categoryService;

        public CategoryServiceUnitTest()
        {
            _categoryService = new CategoryService(validatorMock.Object, productValidatorMock.Object, categoryValidatorMock.Object);
        }

        [TestMethod]
        public void Create__throws_exception_if_name_is_null()
        {
            var dummyCategory = new Models.Category();

            categoryValidatorMock.Setup((m) => m.ValidateName(It.IsAny<Models.Category>()))
                .Throws<Validate.CategoryNameException>();

            Assert.ThrowsException<Validate.CategoryNameException>(() 
                => _categoryService.Create(dummyCategory));
        }

        [TestMethod]
        public void Create__throws_exception_if_name_is_empty()
        {
            var dummyCategory = new Models.Category() { Name = string.Empty };

            categoryValidatorMock.Setup((m) => m.ValidateName(It.IsAny<Models.Category>()))
                .Throws<Validate.CategoryNameException>();

            Assert.ThrowsException<Validate.CategoryNameException>(()
                => _categoryService.Create(dummyCategory));
        }

        [TestMethod]
        public void Create__throws_exception_if_description_is_null()
        {
            var dummyCategory = new Models.Category();

            categoryValidatorMock.Setup((m) => m.ValidateName(It.IsAny<Models.Category>()))
                .Throws<Validate.CategoryNameException>();

            Assert.ThrowsException<Validate.CategoryNameException>(()
                => _categoryService.Create(dummyCategory));
        }

        [TestMethod]
        public void Create__throws_exception_if_description_is_empty()
        {
            var dummyCategory = new Models.Category() { Description = string.Empty };

            categoryValidatorMock.Setup((m) => m.ValidateName(It.IsAny<Models.Category>()))
                .Throws<Validate.CategoryNameException>();

            Assert.ThrowsException<Validate.CategoryNameException>(()
                => _categoryService.Create(dummyCategory));
        }

        [TestMethod]
        public void Create__returns_new_created_object_id_if_process_is_successfuly_completed()
        {
            var dummyCategory = new Models.Category();

            string objectId = "61c640992956533d692e2766";

            categoryValidatorMock.Setup((m) => m.ValidateName(It.IsAny<Models.Category>()));
            categoryValidatorMock.Setup((m) => m.ValidateDescription(It.IsAny<Models.Category>()));
            categoryRepositoryMock.Setup((m) => m.Add(It.IsAny<Repositories.MongoEntities.Category>()))
                .Returns(objectId);

            _categoryService.Create(dummyCategory);
            Assert.AreEqual(objectId, dummyCategory.Id);
        }

        [TestMethod]
        public void Delete__throws_exception_if_given_id_is_not_a_valid_object_id()
        {
            validatorMock.Setup(m => m.ValidateObjectId(It.IsAny<string>()))
                .Throws<Validate.ObjectIdException>();

            Assert.ThrowsException<Validate.ObjectIdException>(()
                => _categoryService.Delete("abcdefgh"));
        }

        [TestMethod]
        public void Delete__throws_exception_if_given_id_exist_in_database()
        {
            validatorMock.Setup(m => m.ValidateObjectId(It.IsAny<string>()));
            categoryRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()));
            categoryRepositoryMock.Setup(m => m.IsExist(It.IsAny<string>()))
                .Returns(false);

            Assert.ThrowsException<Validate.CategoryCannotBeFoundException>(()
                => _categoryService.Delete("abcdefgh"));
        }

        [TestMethod]
        public void Delete__deletes_object_with_given_id()
        {
            Setup();
            validatorMock.Setup(m => m.ValidateObjectId(It.IsAny<string>()));
            categoryRepositoryMock.Setup(m => m.Delete(It.IsAny<string>()));
            categoryRepositoryMock.Setup(m => m.IsExist(It.IsAny<string>()))
                .Returns(true);

            _categoryService.Delete("61c640992956533d692e2766");
        }

        [TestMethod]
        public void Revise__throws_exception_if_object_id_is_not_valid()
        {
            var dummyCategory = new Models.Category() { Description = string.Empty };

            validatorMock.Setup((m) => m.ValidateObjectId(It.IsAny<string>()))
                .Throws<Validate.ObjectIdException>();

            Assert.ThrowsException<Validate.ObjectIdException>(() => 
                _categoryService.Revise(dummyCategory));

        }

        [TestMethod]
        public void Revise__throws_exception_if_update_conditions_are_not_meet()
        {
            var dummyCategory = new Models.Category() { Description = string.Empty };

            validatorMock.Setup((m) => m.ValidateObjectId(It.IsAny<string>()));
            categoryValidatorMock.Setup((m) => m.ValidateUpdate(It.IsAny<Models.Category>()))
                .Throws<Validate.CategoryCannotUpdate>();

            Assert.ThrowsException<Validate.CategoryCannotUpdate>(() 
                => _categoryService.Revise(dummyCategory));
        }

        [TestMethod]
        public void Revise__updates_given_object()
        {
            Setup();
            var dummyCategory = new Models.Category() { Description = string.Empty };
            validatorMock.Setup((m) => m.ValidateObjectId(It.IsAny<string>()));
            categoryRepositoryMock.Setup((m)=> m.Get(It.IsAny<string>()))
                .Returns(new Repositories.MongoEntities.Category());
            categoryValidatorMock.Setup((m) => m.ValidateUpdate(It.IsAny<Models.Category>()));
            categoryUpdatedDomainEventHandler.Setup((m) => m.Handle(It.IsAny<DomainEvent.Models.CategoryUpdated>()));

            var updatedCategory = new Models.Category() { Name = "Name", Description = "Description" };
            _categoryService.Revise(updatedCategory);

            Assert.AreEqual("Name", updatedCategory.Name);
            Assert.AreEqual("Description", updatedCategory.Description);
        }

        //TODO: Update unit tests.

    }
}
