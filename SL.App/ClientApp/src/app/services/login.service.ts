import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { UserAccountDto } from '../models/dto/user-account-dto.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

    getUserAccountByCookie(id: string): Observable<HttpResponse<UserAccountDto>> {
      return this.http.put<UserAccountDto>(`/api/userAccount/${id}`, null, { observe: 'response'});
    }

    getUserAccountByLogin(name: string, email: string): Observable<HttpResponse<UserAccountDto>> {
      return this.http.post<UserAccountDto>(`/api/UserAccount`, { name: name, email: email }, { observe: 'response'});
    }
}

