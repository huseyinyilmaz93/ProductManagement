using Microsoft.AspNetCore.Mvc;
using PM.Api.Exceptions;
using PM.Api.Models;
using PM.Api.Repositories.Implementations;
using PM.Api.Services.Implementations;
using PM.Api.Validators;

namespace PM.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductValidator _productValidator;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductService productService,
                             IProductValidator productValidator,
                             IProductRepository productRepository,
                             ICategoryRepository categoryRepository)
    {
        _productService = productService;
        _productValidator = productValidator;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Returns a product associated with given id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>A product object if exists</returns>
    /// <response code="200">Returns product object</response>
    /// <response code="204">Returns if product object doesnt exist with given id</response>    
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpGet]
    public IActionResult Get([FromQuery]string id)
    {
        var productDocument = _productRepository.Get(id);
        if (productDocument == null)
            throw new Validate.ProductCannotBeFoundException(id);

        var categoryDocument = _categoryRepository.Get(productDocument.CategoryId);
        if (categoryDocument == null)
            throw new Validate.ProductCannotBeFoundException(id);

        //TODO: null check
        var product = new Product()
        {
            Id = productDocument.Id,
            Currency = productDocument.Currency,
            Description = productDocument.Description,
            Name = productDocument.Name,
            Price = productDocument.Price,
            Category = new Models.Category
            {
                Description = categoryDocument.Description,
                Id = categoryDocument.Id,
                Name = categoryDocument.Name
            }
        };

        return Ok(product);
    }


    /// <summary>
    /// Returns all products matches with given parameters
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Returns all matched products after given filtering operation</returns>
    /// <response code="200">Returns product array</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpGet("list")]
    public IActionResult List([FromQuery] Dtos.ListProduct product)
    {
        //TODO: Automapper
        return Ok(_productRepository.List(new Repositories.MongoEntities.Product()
        {
            CategoryId = product.CategoryId ?? string.Empty,
            Currency = product.Currency ?? string.Empty,
            Description = product.Description ?? string.Empty,
            Name = product.Name ?? string.Empty
        }));
    }

    /// <summary>
    /// Inserts a product object with given parameters
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Returns identity of new created object</returns>
    /// <response code="201">Returns identity of new created product</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpPost]
    public IActionResult Insert([FromBody] Dtos.InsertProduct product)
    {
        //TODO: Automapper
        Product newProduct = new Product()
        {
            Currency = product.Currency ?? string.Empty,
            Description = product.Description ?? string.Empty,
            Name = product.Description ?? string.Empty,
            Price = product.Price ?? default(decimal),
            Category = new Category()
            {
                Id = product.CategoryId ?? string.Empty,
            }
        };
        _productService.Create(newProduct);
        return Created(string.Empty, newProduct.Id);
    }

    /// <summary>
    /// Updates a product object with given id if exists
    /// </summary>
    /// <param name="product"></param>
    /// <returns>Returns identity of new created object</returns>
    /// <response code="200">Update process successfuly completed</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpPut]
    public IActionResult Update([FromBody] Dtos.UpdateProduct product)
    {
        //TODO: Automapper
        _productService.Update(new Product()
        {
            Id = product.Id ?? string.Empty,
            Currency = product.Currency ?? string.Empty,
            Description = product.Description ?? string.Empty,
            Name = product.Name ?? string.Empty,
            Price = product.Price ?? default(decimal),
            Category = new Category()
            {
                Id = product.CategoryId ?? string.Empty,
            }
        });
        return Ok();
    }

    /// <summary>
    /// Deletes a product associated with given id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <response code="200">Delete process successfuly completed</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpDelete]
    public IActionResult Delete([FromQuery] string id)
    {
        _productService.Delete(id);
        return Ok();
    }

}

