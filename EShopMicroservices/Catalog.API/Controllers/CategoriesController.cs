using Catalog.API.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Features.Categories.Commands.CreateCategory;
using Catalog.Core.Features.Categories.Commands.DeleteCategory;
using Catalog.Core.Features.Categories.Commands.UpdateCategory;
using Catalog.Core.Features.Categories.Queries.GetAllCategories;
using Catalog.Core.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories.Select(ToDto));
        }

        // GET api/categories/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetById(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category is null) return NotFound();
            return Ok(ToDto(category));
        }

        // POST api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest request)
        {
            var category = await _mediator.Send(new CreateCategoryCommand(request.Name, request.Description));
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, ToDto(category));
        }

        // PUT api/categories/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _mediator.Send(new UpdateCategoryCommand(id, request.Name, request.Description));
            if (category is null) return NotFound();
            return Ok(ToDto(category));
        }

        // DELETE api/categories/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }

        private static CategoryDto ToDto(Category c) => new(c.Id, c.Name, c.Description);
    }
}
