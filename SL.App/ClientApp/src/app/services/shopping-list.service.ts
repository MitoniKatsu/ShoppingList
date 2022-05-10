import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ListItemDto } from '../models/dto/list-item-dto.model';
import { ShoppingListDto } from '../models/dto/shopping-list-dto.model';
import { ShoppingListItemDto } from '../models/dto/shopping-list-item-dto.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {

  constructor(private http: HttpClient) { }

  getListItems(): Observable<HttpResponse<ListItemDto[]>> {
    return this.http.get<ListItemDto[]>(`/api/ListItem`, { observe: 'response'});
  }
  getShoppingList(userId: string): Observable<HttpResponse<ShoppingListDto>> {
    return this.http.get<ShoppingListDto>(`/api/ShoppingList/${userId}`, {observe: 'response'});
  }
  addItemToShoppingList(shoppingListId: string, listItem: ListItemDto): Observable<HttpResponse<ShoppingListDto>> {
    return this.http.put<ShoppingListDto>(`/api/ShoppingList/${shoppingListId}`, listItem, {observe: 'response'});
  }
  deleteItemFromShoppingList(shoppingListItemId: string): Observable<HttpResponse<null>> {
    return this.http.delete<null>(`/api/ShoppingListItem/${shoppingListItemId}`, {observe: 'response'});
  }
  toggleItemComplete(shoppingListItemId: string): Observable<HttpResponse<ShoppingListItemDto>> {
    return this.http.put<ShoppingListItemDto>(`/api/ShoppingListItem/${shoppingListItemId}`, null, {observe: 'response'});
  }
}
