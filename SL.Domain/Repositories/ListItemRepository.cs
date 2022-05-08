using SL.Data;
using SL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.Repositories
{
    public interface IListItemRepository
    {
        IList<ListItem> GetList();
    }

    public class ListItemRepository : IListItemRepository
    {
        private IShoppingContext _dbContext { get; set; }

        public ListItemRepository(IShoppingContext context)
        {
            _dbContext = context;
        }

        public IList<ListItem> GetList()
        {
            return _dbContext.ListItems
                .ToList();
        }
    }
}
