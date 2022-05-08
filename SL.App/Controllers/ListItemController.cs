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
    public class ListItemController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly ILogger _logger;
        public ListItemController(IShoppingListService service, ILogger<ListItemController> logger)
        {
            _shoppingListService = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            try
            {
                return _shoppingListService.GetAutoCompleteList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
