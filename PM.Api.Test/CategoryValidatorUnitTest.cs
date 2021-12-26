using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PM.Api.Exceptions;
using PM.Api.Test.Base;
using PM.Api.Validators;

namespace PM.Api.Test.ValidationTests
{
    [TestClass]
    public class CategoryValidatorUnitTest : BaseUnitTest
    {
        ICategoryValidator _categoryValidator;

        public CategoryValidatorUnitTest()
        {
            _categoryValidator = new CategoryValidator(productRepositoryMock.Object, categoryRepositoryMock.Object);
        }

        [TestMethod]
        public void ValidateId__throws_exception_if_id_cannot_cast_an_object_id()
        {
            Assert.ThrowsException<Validate.CategoryIdException>( () 
                => _categoryValidator.ValidateId(new Models.Category() { Id = null }));
        }

        [TestMethod]
        public void ValidateId__does_not_throw_any_exception_if_id_can_cast_an_object_id()
        {
            _categoryValidator.ValidateId(new Models.Category() { Id = "61c377db664d09f15c219d11" });
        }

        //TODO: ValidateName, ValidateDescription, ValidateUpdate
        [TestMethod]
        public void ValidateCategoryExistance__throws_exception_if_given_category_id_cannot_found()
        {
            base.categoryRepositoryMock.Setup(m => m.IsExist(It.IsAny<string>()))
                .Returns(false);

            Assert.ThrowsException<Validate.CategoryCannotBeFoundException>( ()
                => _categoryValidator.ValidateCategoryExistance(new Models.Product() { Category = new Models.Category() { Id = "1" } } ));
        }

        [TestMethod]
        public void ValidateCategoryExistance__does_not_throw_any_exception_if_given_category_id_exists()
        {
            base.categoryRepositoryMock.Setup(m => m.IsExist(It.IsAny<string>()))
                .Returns(true);

            _categoryValidator.ValidateCategoryExistance(new Models.Product() { Category = new Models.Category() { Id = "1" } });
        }

    }

    //TODO: IValidator, IProductValidator
}
