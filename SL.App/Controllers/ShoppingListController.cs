using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SL.Data.Models;
using SL.Domain.DTO;
using SL.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SL.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly ILogger _logger;
        public ShoppingListController(IShoppingListService service, ILogger<ShoppingListController> logger)
        {
            _shoppingListService = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId:required}")]
        public async Task<IActionResult> GetShoppingList([FromRoute] Guid userId)
        {
            try
            {
                return await _shoppingListService.LoadShoppingList(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }

        [HttpPut]
        [Route("{shoppingListId:required}")]
        public async Task<IActionResult> AddToShoppingList([FromRoute] Guid shoppingListId, [FromBody][Required] AddToShoppingListDto dto)
        {
            try
            {
                return await _shoppingListService.AddToShoppingList(shoppingListId, dto.ListItemId, dto.ItemName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
