using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PM.Api.DomainEvent;
using PM.Api.DomainEvent.Base;
using PM.Api.IoC;
using PM.Api.MemoryCacher;
using PM.Api.Repositories.Implementations;
using PM.Api.Validators;
using System.Collections.Generic;

namespace PM.Api.Test.Base
{
    public class BaseUnitTest
    {
        protected Mock<IValidator> validatorMock;
        protected Mock<IMemoryCacher> memoryCacherMock;
        protected Mock<IDomainEventModel> domainEventModel;
        protected Mock<ICategoryRepository> categoryRepositoryMock;
        protected Mock<IProductRepository> productRepositoryMock;
        protected Mock<IProductValidator> productValidatorMock;
        protected Mock<ICategoryValidator> categoryValidatorMock;
        protected Mock<ICategoryUpdatedDomainEventHandler> categoryUpdatedDomainEventHandler;

        protected IConfiguration configuration;

        public BaseUnitTest()
        {
            Setup();
        }

        public void Setup()
        {
            validatorMock = new Mock<IValidator>();
            memoryCacherMock = new Mock<IMemoryCacher>();
            domainEventModel = new Mock<IDomainEventModel>();
            categoryRepositoryMock = new Mock<ICategoryRepository>();
            productRepositoryMock = new Mock<IProductRepository>();
            productValidatorMock = new Mock<IProductValidator>();
            categoryValidatorMock = new Mock<ICategoryValidator>();
            categoryUpdatedDomainEventHandler = new Mock<ICategoryUpdatedDomainEventHandler>();

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(validatorMock.Object);
            serviceCollection.AddSingleton(memoryCacherMock.Object);
            serviceCollection.AddSingleton(categoryRepositoryMock.Object);
            serviceCollection.AddSingleton(productRepositoryMock.Object);
            serviceCollection.AddSingleton(categoryValidatorMock.Object);
            serviceCollection.AddSingleton(domainEventModel.Object);
            serviceCollection.AddSingleton(categoryUpdatedDomainEventHandler.Object);

            var configurationBuild = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            configuration = configurationBuild as IConfiguration;

            serviceCollection.AddSingleton(configuration);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            IoCHelper.SetServiceProvider(serviceProvider);


            IoCHelper.Resolve<ICategoryRepository>();
        }
    }
}
