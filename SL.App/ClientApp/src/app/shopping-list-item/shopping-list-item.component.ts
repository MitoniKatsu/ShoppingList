import { Component, Input, OnInit } from '@angular/core';
import { ShoppingListItemDto } from '../models/dto/shopping-list-item-dto.model';

@Component({
  selector: 'app-shopping-list-item',
  templateUrl: './shopping-list-item.component.html',
  styleUrls: ['./shopping-list-item.component.css']
})
export class ShoppingListItemComponent implements OnInit {

  @Input() shoppingListItem: ShoppingListItemDto = null;
  constructor() { }

  ngOnInit() {
  }

  get shoppingListItemName() {
    return this.shoppingListItem !== null ? this.shoppingListItem.listItem.name : '';
  }

}
