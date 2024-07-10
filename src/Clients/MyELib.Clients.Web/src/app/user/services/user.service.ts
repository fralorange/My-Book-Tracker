import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../models/user.model';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../../environments/environment.development';
import { TokenService } from '../../auth/services/token.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private url = "User";

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getCurrentUser(): Observable<UserModel> | null {
    var token = this.tokenService.getToken();
    if (!token)
      return null;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.get<UserModel>(`${environment.apiUrl}/${this.url}`, { headers: headers });
  }

  putUser(user: UserModel, password: string) {
    var token = this.tokenService.getToken();
    if (!token)
      return;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    this.http.put(`${environment.apiUrl}/${this.url}/${user.id}`, { username: user.username, email: user.email, password: password }, { headers: headers }).subscribe();
  }

  deleteUser(id: string) {
    var token = this.tokenService.getToken();
    if (!token)
      return;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    this.http.delete(`${environment.apiUrl}/${this.url}/${id}`, {
      headers: headers
    }).subscribe();
  }
}
