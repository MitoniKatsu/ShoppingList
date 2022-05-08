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
    public class ShoppingListItemController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly ILogger _logger;
        public ShoppingListItemController(IShoppingListService service, ILogger<ShoppingListItemController> logger)
        {
            _shoppingListService = service;
            _logger = logger;
        }

        [HttpPut]
        [Route("{shoppingListItemId:required}")]
        public async Task<IActionResult> ToggleComplete([FromRoute] Guid shoppingListItemId)
        {
            try
            {
                return await _shoppingListService.UpdateShoppingListItem(shoppingListItemId, Domain.Enum.ShoppingListItemAction.TOGGLE_COMPLETE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }

        [HttpDelete]
        [Route("{shoppingListItemId:required}")]
        public async Task<IActionResult> DeleteShoppingListItem([FromRoute] Guid shoppingListItemId)
        {
            try
            {
                return await _shoppingListService.UpdateShoppingListItem(shoppingListItemId, Domain.Enum.ShoppingListItemAction.TOGGLE_COMPLETE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
