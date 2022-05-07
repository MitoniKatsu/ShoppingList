﻿using Microsoft.AspNetCore.Http;
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
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ILogger _logger;
        public UserAccountController(IUserAccountService service, ILogger<UserAccountController> logger)
        {
            _userAccountService = service;
            _logger = logger;
        }

        /// <summary>
        /// Logs the provided user account in by userId and updates last logged in datetime
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId:required}")]
        public async Task<IActionResult> LoginById([FromRoute] Guid userId)
        {
            try
            {
                return await _userAccountService.LoginWithId(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }

        /// <summary>
        /// Logs the provided user account in by email address
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginByEmail([FromBody][Required] UserAccountByEmailDto account)
        {
            try
            {
                return await _userAccountService.LoginWithEmail(account.Name, account.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return new BadRequestObjectResult("An error occured processing your request.");
            }
        }
    }
}
