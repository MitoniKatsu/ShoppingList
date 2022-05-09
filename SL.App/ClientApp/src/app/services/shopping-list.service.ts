import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ListItemDto } from '../models/dto/list-item-dto.model';
import { ShoppingListDto } from '../models/dto/shopping-list-dto.model';

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
}
