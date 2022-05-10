import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { concatMap, map, mergeMap, startWith, tap } from 'rxjs/operators';
import { ListItemDto } from '../models/dto/list-item-dto.model';
import { ShoppingListItemDto } from '../models/dto/shopping-list-item-dto.model';
import { ApplicationActions } from '../ngxs-core/application/application.action';
import { ApplicationStateSelector } from '../ngxs-core/application/application.selector';
import { ShoppingListActions } from '../ngxs-core/sl/sl.action';
import { ShoppingListStateSelector } from '../ngxs-core/sl/sl.selector';

@Component({
  selector: 'app-shopping-list-input',
  templateUrl: './shopping-list-input.component.html',
  styleUrls: ['./shopping-list-input.component.css']
})
export class ShoppingListInputComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private store: Store
    ) { }

  shoppingList: FormGroup;

  @Select(ShoppingListStateSelector.getShoppingListItems) $shoppingListItems: Observable<ShoppingListItemDto[]>;

  @Select(ApplicationStateSelector.getListItems) $listItems: Observable<ListItemDto[]>;

  $filteredOptions: Observable<ListItemDto[]>;

  ngOnInit() {
    this.initForm();
    this.$filteredOptions = this.filterOptions();
  }

  initForm() {
    this.shoppingList = this.fb.group({
      itemInput: ['', [Validators.required]]
    });
  }

  filterOptions(): Observable<ListItemDto[]> {
    const input = this.shoppingList.controls['itemInput'];

    return input.valueChanges.pipe(
      startWith(''),
      mergeMap(change => this.$listItems.pipe(
        map(m => m.filter(f => f.name.toLowerCase().includes(change.toLowerCase())))
      ))
    );
  }

  clearInput() {
    const input = this.shoppingList.controls['itemInput'];
    input.setValue('');
  }

  addItem() {
    if (this.shoppingList.valid && this.shoppingList.dirty) {
      const input = this.shoppingList.controls['itemInput'];
      if (input.value && input.value.length > 0) {
        this.store.dispatch(new ShoppingListActions.AddToShoppingList(input.value))
        .subscribe();
      }
    }
  }

  get submitDisabled() {
    const input = this.shoppingList.controls['itemInput'];
    if (this.shoppingList.valid && this.shoppingList.dirty && input && input.value.length > 0) {
      return false;
    }
    return true;
  }
}
