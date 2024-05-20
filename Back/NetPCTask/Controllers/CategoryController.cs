using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using NetPCTask.Services;
using NetPCTask.Dto;
using NetPCTask.Models;

namespace NetPCTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories() 
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryService.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            if (!_categoryService.CategoryExists(id))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryService.GetCategory(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryDto category) 
        { 
            if (category == null)
                return BadRequest(ModelState);

            var cat = _categoryService.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == category.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (cat != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);

            if (!_categoryService.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto category)
        {
            if (category == null)
                return BadRequest(ModelState);

            if (category.Id != 0 && category.Id != categoryId)
                return BadRequest("Id in URL doesn't much id in request body");

            if (category.Id == 0)
                category.Id = categoryId;

            if (!_categoryService.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(category);

            if (!_categoryService.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating the category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryService.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryService.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryService.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the category");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
