using SL.Data;
using SL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SL.Domain.Repositories
{
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Creates a new user account with provided name and email.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserAccount> Create(string name, string email);

        /// <summary>
        /// Get a list of user accounts
        /// </summary>
        /// <returns></returns>
        IList<UserAccount> GetList();

        /// <summary>
        /// Get a user account by Email, or null if not found.
        /// Throws <see cref="InvalidOperationException"/> if email is null or empty.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAccount GetUserAccountByEmail(string email);

        /// <summary>
        /// Get a user account by Id, or null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAccount GetUserAccountById(Guid id);

        /// <summary>
        /// Updates a user account LastLogin to current datetime.
        /// /// Throws <see cref="InvalidOperationException"/> if provided account is null or empty.
        /// /// Throws <see cref="KeyNotFoundException"/> if provided account is not found.
        /// </summary>
        /// <param name="updatedAccount"></param>
        /// <returns></returns>
        Task<UserAccount> UpdateLastLogin(UserAccount updatedAccount);
    }

    public class UserAccountRepository : IUserAccountRepository
    {
        private IShoppingContext _dbContext { get; set; }

        public UserAccountRepository(IShoppingContext context)
        {
            _dbContext = context;
        }
        
        public IList<UserAccount> GetList()
        {
            return _dbContext.UserAccounts
                .ToList();
        }

        public UserAccount GetUserAccountById(Guid id)
        {
            var account = _dbContext.UserAccounts.ToList()
                .FirstOrDefault(o => o.Id.Equals(id));

            return account;
        }

        public UserAccount GetUserAccountByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException($"Unable to get user by email. Provided email is {(email == null ? "null" : "empty")}.");
            }

            var account = _dbContext.UserAccounts.ToList()
                .FirstOrDefault(o => o.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            return account;
        }

        public async Task<UserAccount> Create(string name, string email)
        {
            var now = DateTime.UtcNow;
            var account = new UserAccount()
            {
                Name = name,
                Email = email,
                CreatedOn = now,
                LastLogin = now
            };

            _dbContext.UserAccounts.Add(account);

            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<UserAccount> UpdateLastLogin(UserAccount updatedAccount)
        {
            var now = DateTime.UtcNow;
            if (updatedAccount == null)
            {
                throw new InvalidOperationException("Unable to update last login. Provided account is null.");
            }
            var account = _dbContext.UserAccounts.ToList()
                .FirstOrDefault(o => o.Id.Equals(updatedAccount.Id));

            if (account == null)
            {
                throw new KeyNotFoundException("Unable to update last login. Provided account not found.");
            }

            account.LastLogin = now;

            await _dbContext.SaveChangesAsync();

            return account;
        }
    }
}
