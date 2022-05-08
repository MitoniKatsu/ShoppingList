using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.DTO
{
    public class AddToShoppingListDto
    {
        public Guid? ListItemId { get; set; }
        public string ItemName { get; set; }
    }
}
