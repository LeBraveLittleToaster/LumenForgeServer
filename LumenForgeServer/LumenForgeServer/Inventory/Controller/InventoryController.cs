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
    /// <summary>
    /// HTTP API for inventory operations.
    /// </summary>
    /// <remarks>
    /// Routes are under <c>api/v1</c> and require authenticated access.
    /// </remarks>
    [Authorize]
    [Route("api/v1/")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// Initializes the controller with required services.
        /// </summary>
        /// <param name="logger">Logger for inventory endpoints.</param>
        /// <param name="inventoryService">Inventory service used for operations.</param>
        public InventoryController(ILogger<InventoryController> logger, InventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryService _inventoryService;

        /// <summary>
        /// Returns a sample category payload for the provided device guid.
        /// </summary>
        /// <param name="deviceGuid">Device guid from the route.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A stubbed category payload.</returns>
        /// <remarks>
        /// This endpoint currently returns a hard-coded category and ignores the device guid.
        /// </remarks>
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

        /// <summary>
        /// Creates a sample category with hard-coded data.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created category payload.</returns>
        /// <exception cref="LumenForgeServer.Common.Exceptions.UniqueConstraintException">
        /// Thrown when the category name violates a unique constraint.
        /// </exception>
        [HttpGet("add")]
        public async Task<ActionResult> AddCategories(CancellationToken ct)
        {
            var cat = await _inventoryService.AddCategory(
                new CreateCategoryDTO { Name = "Test Name", Description = "Test Description" }, ct);
            return new JsonResult(CategoryFactory.FromCategory(cat));
        }

        /// <summary>
        /// Creates a category from the provided payload.
        /// </summary>
        /// <param name="dto">Payload containing category details.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A category view payload.</returns>
        /// <remarks>
        /// Returns HTTP 400 when the payload fails validation.
        /// </remarks>
        /// <exception cref="LumenForgeServer.Common.Exceptions.UniqueConstraintException">
        /// Thrown when the category name violates a unique constraint.
        /// </exception>
        [Produces("application/json")]
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CategoryViews>> CreateCategory(CreateCategoryDTO dto, CancellationToken ct)
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
