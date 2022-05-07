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
        Task<UserAccount> Create(string name, string email);
        IList<UserAccount> GetList();
        UserAccount GetUserAccountByEmail(string email);
        UserAccount GetUserAccountById(Guid id);
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
            var account = _dbContext.UserAccounts
                .FirstOrDefault(o => o.Id.Equals(id));

            return account;
        }

        public UserAccount GetUserAccountByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException($"Unable to get user by email. Provided email is {(email == null ? "null" : "empty")}.");
            }

            var account = _dbContext.UserAccounts
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
            var account = _dbContext.UserAccounts
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
