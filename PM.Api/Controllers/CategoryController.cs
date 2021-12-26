using Microsoft.AspNetCore.Mvc;
using PM.Api.Models;
using PM.Api.Repositories.Implementations;
using PM.Api.Services.Implementations;

namespace PM.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICategoryRepository _categoryRepository;

    //TODO: object id control.
    public CategoryController(ICategoryService categoryService,
                              ICategoryRepository categoryRepository)
    {
        _categoryService = categoryService;
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Returns a category associated with given id
    /// </summary>
    /// <param name="id">Category id</param>
    /// <returns>A category object if exists</returns>
    /// <response code="200">Returns category object</response>
    /// <response code="204">Returns if category object doesnt exist with given id</response>
    /// <response code="400">Returns if any validation fails.</response>
    /// <response code="500">Something wrong on server</response>
    [HttpGet]
    public IActionResult Get(string id)
    {
        return Ok(_categoryRepository.Get(id));
    }

    /// <summary>
    /// Returns all categories matches with given parameters
    /// </summary>
    /// <param name="category"></param>
    /// <returns>Returns all matched categories after given filtering operation</returns>
    /// <response code="200">Returns category array</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpGet("list")]
    public IActionResult List([FromQuery] Dtos.ListCategory category)
    {
        //TODO: Automapper
        return Ok(_categoryRepository.List(new Repositories.MongoEntities.Category()
        {
            Description = category.Description ?? string.Empty,
            Name = category.Name ?? string.Empty
        }));
    }

    /// <summary>
    /// Inserts a category object with given parameters
    /// </summary>
    /// <param name="category"></param>
    /// <returns>Returns identity of new created object</returns>
    /// <response code="201">Returns identity of new created product</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpPost]
    public IActionResult Insert([FromBody] Dtos.InsertCategory category)
    {
        //TODO: Automapper
        Category newCategory = new Category()
        {
            Description = category.Description ?? string.Empty,
            Name = category.Name ?? string.Empty,
        };
        _categoryService.Create(newCategory);
        return Created(string.Empty, newCategory.Id);
    }

    /// <summary>
    /// Updates a category object with given id if exists
    /// </summary>
    /// <param name="category"></param>
    /// <returns>Returns identity of new created object</returns>
    /// <response code="200">Update process successfuly completed</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpPut]
    public IActionResult Update([FromBody] Dtos.UpdateCategory category)
    {
        //TODO: Automapper
        _categoryService.Revise(new Category()
        {
            Id = category.Id ?? string.Empty,
            Description = category.Description ?? string.Empty,
            Name = category.Name ?? string.Empty,
        });
        return Ok();
    }

    /// <summary>
    /// Deletes a category associated with given id
    /// </summary>
    /// <param name="id">Category id</param>
    /// <response code="200">Delete process successfuly completed</response>
    /// <response code="400">Returns if any validation fails</response>
    /// <response code="500">Something wrong on server</response>
    [HttpDelete]
    public IActionResult Delete(string id)
    {
        _categoryService.Delete(id);
        return Ok();
    }
}

