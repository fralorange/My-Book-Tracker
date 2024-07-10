import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenService } from '../../auth/services/token.service';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { LibraryUserModel } from '../models/library-user.model';

@Injectable({
  providedIn: 'root'
})
export class LibraryUserService {
  private url = "LibraryUser";

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getLibraryUserById(id: string): Observable<LibraryUserModel> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.get<LibraryUserModel>(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }
  
  initLibraryUser(libraryId: string): Observable<any> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.post(`${environment.apiUrl}/${this.url}/init`, { libraryId: libraryId }, { headers: headers });
  }

  grantAccessToUser(libraryId: string, userId: string, role: number) : Observable<any> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.post(`${environment.apiUrl}/${this.url}/grant`, { libraryId: libraryId, userId: userId, role: role }, { headers: headers });
  }

  patchLibraryUser(id: string, libraryId: string, role: number) : Observable<any> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.patch(`${environment.apiUrl}/${this.url}/${id}`, { libraryId: libraryId, role: role }, { headers: headers });
  }

  deleteLibraryUser(id: string) {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.delete(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }
}
