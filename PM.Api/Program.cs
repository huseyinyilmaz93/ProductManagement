using PM.Api.DomainEvent;
using PM.Api.IoC;
using PM.Api.Logger;
using PM.Api.Managers;
using PM.Api.MemoryCacher;
using PM.Api.Middleware;
using PM.Api.Repositories;
using PM.Api.Repositories.Implementations;
using PM.Api.Services;
using PM.Api.Services.Implementations;
using PM.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//TODO: transient vs singleton.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c 
    => c.IncludeXmlComments(string.Format(@"{0}PM.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory)));

builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IProductService, ProductService>();


builder.Services.AddSingleton<IValidator, Validator>();
builder.Services.AddSingleton<PM.Api.Logger.ILogger, ConsoleLogger>();

builder.Services.AddSingleton<ICategoryValidator, CategoryValidator>();
builder.Services.AddSingleton<IProductValidator, ProductValidator>();

builder.Services.AddSingleton<ICategoryRepository, MongoCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, MongoProductRepository>();

builder.Services.AddSingleton<IMemoryCacher, RedisMemoryCacher>();
builder.Services.AddSingleton<IMqManager, DummyMqManager>();

builder.Services.AddSingleton<ICategoryUpdatedDomainEventHandler, CategoryUpdatedDomainEventHandler>();
builder.Services.AddSingleton<IProductDeletedDomainEventHandler, ProductDeletedDomainEventHandler>();
builder.Services.AddSingleton<IProductUpdatedEventDomainHandler, ProductUpdatedEventDomainHandler>();

var app = builder.Build();

IoCHelper.SetServiceProvider(app.Services);

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<PMApiMiddleware>();

app.Run();

