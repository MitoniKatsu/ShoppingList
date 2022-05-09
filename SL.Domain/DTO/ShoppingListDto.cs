using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.DTO
{
    public class ShoppingListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IList<ShoppingListItemDto> Items { get; set; }
    }

    public class ShoppingListItemDto
    {
        public Guid Id { get; set; }
        public Guid ShoppingListId { get; set; }
        public bool Complete { get; set; }
        public ListItemDto ListItem { get; set; }
    }

    public class ListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
