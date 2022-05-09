using SL.Data.Models;
using SL.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.Extensions
{
    public static class MappingExtensions
    {
        public static ListItemDto ToDto(this ListItem entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = new ListItemDto
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return dto;
        }

        public static IList<ListItemDto> ToDto(this IList<ListItem> entities)
        {
            return entities.Select(o => o.ToDto()).ToList();
        }

        public static ShoppingListItemDto ToDto(this ShoppingListItem entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = new ShoppingListItemDto
            {
                Id = entity.Id,
                ShoppingListId = entity.ShoppingListId,
                Complete = entity.Complete,
                ListItem = entity.ListItem.ToDto()
            };

            return dto;
        }

        public static IList<ShoppingListItemDto> ToDto(this IList<ShoppingListItem> entities)
        {
            return entities.Select(o => o.ToDto()).ToList();
        }

        public static ShoppingListDto ToDto(this ShoppingList entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = new ShoppingListDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Items = entity.Items.ToList().ToDto()
            };

            return dto;
        }

        public static UserAccountDto ToDto(this UserAccount entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = new UserAccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Returning = (entity.CreatedOn < entity.LastLogin) ? true : false
            };

            return dto;
        }
    }
}
