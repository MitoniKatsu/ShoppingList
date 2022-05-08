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

        /// <summary>
        /// Updates an existing shoppingListItem's complete status, toggling between true and false values
        /// </summary>
        /// <param name="shoppingListItemId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes a shoppingListItem from an existing shoppingList
        /// </summary>
        /// <param name="shoppingListItemId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{shoppingListItemId:required}")]
        public async Task<IActionResult> DeleteShoppingListItem([FromRoute] Guid shoppingListItemId)
        {
            try
            {
                return await _shoppingListService.UpdateShoppingListItem(shoppingListItemId, Domain.Enum.ShoppingListItemAction.DELETE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
