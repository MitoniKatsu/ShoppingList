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

        /// <summary>
        /// Gets a shopping list by the provided userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a new shoppingListItem to an existing shoppingList.  This can be specified by either listItemid if listitem is pre-exiting, or itemName if listItem is being added.
        /// </summary>
        /// <param name="shoppingListId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{shoppingListId:required}")]
        public async Task<IActionResult> AddToShoppingList([FromRoute] Guid shoppingListId, [FromBody][Required] AddToShoppingListDto dto)
        {
            try
            {
                if (dto.ListItemId == null)
                {
                    return await _shoppingListService.AddToShoppingList(shoppingListId, dto.ItemName);
                }
                else
                {
                    return await _shoppingListService.AddToShoppingList(shoppingListId, dto.ListItemId.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
