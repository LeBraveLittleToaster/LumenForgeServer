using LumenForgeServer.Auth;
using LumenForgeServer.Inventory.Domain;
using LumenForgeServer.Inventory.Dto.Create;
using LumenForgeServer.Inventory.Dto.View;
using LumenForgeServer.Inventory.Factory;
using LumenForgeServer.Inventory.Service;
using LumenForgeServer.Inventory.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumenForgeServer.Inventory.Controller
{
    [Authorize]
    [Route("api/v1/")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        public InventoryController(ILogger<InventoryController> logger, InventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryService _inventoryService;

        [Authorize(Policy =AuthConstants.POLICY_ADMIN_ONLY)]
        [HttpGet("{deviceGuid}")]
        public ActionResult GetCategories(string deviceGuid, CancellationToken ct)
        {
            var cat = new Category
            {
                Name = "Test Name",
                Description = "Test Description",
                Guid = Guid.NewGuid(),
            };
            return new JsonResult(CategoryFactory.FromCategory(cat));
        }

        [HttpGet("add")]
        public async Task<ActionResult> AddCategories(CancellationToken ct)
        {
            var cat = await _inventoryService.AddCategory(
                new CreateCategoryDTO { Name = "Test Name", Description = "Test Description" }, ct);
            return new JsonResult(CategoryFactory.FromCategory(cat));
        }

        [Produces("application/json")]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CategoryViewDto>> CreateCategory(CreateCategoryDTO dto, CancellationToken ct)
        {
            if (!CategoryValidator.ValidateCreateCategoryDto(dto))
            {
                return BadRequest("Invalid Input data, name not between 1 and 100 characters");
            }

            _logger.LogInformation($"Creating category {dto.Name}");
            var category = await _inventoryService.AddCategory(dto, ct);
            return new JsonResult(CategoryFactory.FromCategory(category));
        }
    }
}