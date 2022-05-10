import { Component, OnInit } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ShoppingListItemDto } from '../models/dto/shopping-list-item-dto.model';
import { ShoppingListActions } from '../ngxs-core/sl/sl.action';
import { ShoppingListStateSelector } from '../ngxs-core/sl/sl.selector';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css']
})
export class ShoppingListComponent implements OnInit {

  @Select(ShoppingListStateSelector.getShoppingListItems) $shoppingListItems: Observable<ShoppingListItemDto[]>;
  constructor() { }

  ngOnInit() {
  }

}
