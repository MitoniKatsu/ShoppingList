using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Data.Models
{
    public class ShoppingListItem
    {
        public Guid Id { get; set; }
        public Guid ShoppingListId { get; set; }
        public Guid ListItemId { get; set; }
        public bool Complete { get; set; } = false;

        public ListItem ListItem { get; set; }
        public ShoppingList ShoppingList { get; set; }
    }
}
