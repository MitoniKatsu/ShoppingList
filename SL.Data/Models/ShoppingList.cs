using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Data.Models
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; }

        public UserAccount UserAccount { get; set; }
    }
}
