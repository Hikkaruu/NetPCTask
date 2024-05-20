using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetPCTask.Dto;
using NetPCTask.Models;
using NetPCTask.Services;

namespace NetPCTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly SubcategoryService _subcategoryService;

        private readonly IMapper _mapper;

        public SubcategoryController(SubcategoryService subcategoryService, IMapper mapper)
        {
            _subcategoryService = subcategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSubcategories()
        {
            var categories = _mapper.Map<List<SubcategoryDto>>(_subcategoryService.GetSubcategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubcategory(int id)
        {
            if (!_subcategoryService.SubcategoryExists(id))
                return NotFound();

            var subcategory = _mapper.Map<SubcategoryDto>(_subcategoryService.GetSubcategory(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(subcategory);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetSubcategoryByName(string name)
        {

            var subcategory = _mapper.Map<SubcategoryDto>(_subcategoryService.GetSubcategoryByName(name));

            if (subcategory == null)
                return NotFound(new { Message = "Subcategory not found" });         
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(subcategory);
        }

        [HttpPost]
        public IActionResult AddSubcategory([FromBody] SubcategoryDto subcategory)
        {
            if (subcategory == null)
                return BadRequest(ModelState);

            var scat = _subcategoryService.GetSubcategories()
                .Where(c => c.Name.Trim().ToUpper() == subcategory.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (scat != null)
            {
                ModelState.AddModelError("", "subcategory already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subcategoryMap = _mapper.Map<Subcategory>(subcategory);

            if (!_subcategoryService.CreateSubcategory(subcategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult UpdateSubcategory(int subcategoryId, [FromBody] SubcategoryDto subcategory)
        {
            if (subcategory == null)
                return BadRequest(ModelState);

            if (subcategory.Id != 0 && subcategory.Id != subcategoryId)
                return BadRequest("Id in URL doesn't much id in request body");

            if (subcategory.Id == 0)
                subcategory.Id = subcategoryId;

            if (!_subcategoryService.SubcategoryExists(subcategoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var subcategoryMap = _mapper.Map<Subcategory>(subcategory);

            if (!_subcategoryService.UpdateSubcategory(subcategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating the subcategory");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteSubcategory(int subcategoryId)
        {
            if (!_subcategoryService.SubcategoryExists(subcategoryId))
            {
                return NotFound();
            }

            var subcategoryToDelete = _subcategoryService.GetSubcategory(subcategoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_subcategoryService.DeleteSubcategory(subcategoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the subcategory");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetSubcategoriesByCategory(int categoryId)
        {
            var subcategories = await _subcategoryService.GetSubcategoriesByCategory(categoryId);
            return Ok(subcategories);
        }

        
    }
}
