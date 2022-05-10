import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { ShoppingListItemDto } from '../models/dto/shopping-list-item-dto.model';
import { ShoppingListActions } from '../ngxs-core/sl/sl.action';

@Component({
  selector: 'app-shopping-list-item',
  templateUrl: './shopping-list-item.component.html',
  styleUrls: ['./shopping-list-item.component.css']
})
export class ShoppingListItemComponent implements OnInit {

  @Input() shoppingListItem: ShoppingListItemDto = null;

  itemForm: FormGroup;

  constructor(
    private store: Store,
    private fb: FormBuilder
    ) { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.itemForm = this.fb.group({
      complete: [this.shoppingListItem.complete]
    });
  }

  toggleComplete() {
    this.store.dispatch(new ShoppingListActions.ToggleItemComplete(this.shoppingListItem.id))
    .subscribe();
  }

  deleteItem() {
    this.store.dispatch(new ShoppingListActions.DeleteFromShoppingList(this.shoppingListItem.id))
    .subscribe();
  }

  get shoppingListItemName() {
    return this.shoppingListItem !== null ? this.shoppingListItem.listItem.name : '';
  }
}
