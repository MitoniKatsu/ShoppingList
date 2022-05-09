using Microsoft.AspNetCore.Mvc;
using SL.Data.Models;
using SL.Domain.Extensions;
using SL.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.Services
{
    public interface IUserAccountService
    {
        /// <summary>
        /// Attempts to log the user account in with the provided email address.  If name or email address is null or empty, it will return a bad request.
        /// If sucessfully logged in, it will return a 200 response with the updated account.  If name and email are valid, but user does not exist, a new
        /// user account is created, and returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IActionResult> LoginWithEmail(string name, string email);

        /// <summary>
        /// Attempts to log the user account in with the provided userId.  If successful, it will update last login and return a 200 response with the user account.
        /// If  the user account is not found, it will return a bad request.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IActionResult> LoginWithId(Guid userId);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepo;
        public UserAccountService(IUserAccountRepository repository)
        {
            _userAccountRepo = repository;
        }

        public async Task<IActionResult> LoginWithId(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return new BadRequestObjectResult("The provided userId is not valid.");
            }

            var account = _userAccountRepo.GetUserAccountById(userId);

            if (account == null)
            {
                return new NotFoundResult();
            }

            account = await _userAccountRepo.UpdateLastLogin(account);

            return new OkObjectResult(account.ToDto());
        }

        public async Task<IActionResult> LoginWithEmail(string name, string email)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return new BadRequestObjectResult("The provided name and email cannot be empty or null");
            }

            var account = _userAccountRepo.GetUserAccountByEmail(email);

            // only create a new account if unable to retrieve existing account by email
            if (account == null)
            {
                account = await _userAccountRepo.Create(name, email);

                return new OkObjectResult(account.ToDto());
            }

            account = await _userAccountRepo.UpdateLastLogin(account);

            return new OkObjectResult(account.ToDto());
        }
    }
}
