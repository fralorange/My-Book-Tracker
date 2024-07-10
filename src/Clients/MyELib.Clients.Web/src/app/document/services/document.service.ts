import { Injectable } from '@angular/core';
import { TokenService } from '../../auth/services/token.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DocumentModel } from '../models/document.model';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private url = "Document";

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  base64StringToText(base64String: string) {
    const uint8Array = new Uint8Array(atob(base64String).split('').map(c => c.charCodeAt(0)));
    return new TextDecoder('utf-8').decode(uint8Array);
  }

  getDocumentById(id: string): Observable<DocumentModel> | null  {
    var token = this.tokenService.getToken();
    if (!token)
      return null;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.get<DocumentModel>(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }

  postDocument(data: FormData) : Observable<any> | null {
    var token = this.tokenService.getToken();
    if (!token)
      return null;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.post(`${environment.apiUrl}/${this.url}`, data, { headers: headers });
  }

  patchDocument(id: string, name: string): Observable<any> | null {
    var token = this.tokenService.getToken();
    if (!token)
      return null;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.patch(`${environment.apiUrl}/${this.url}/${id}`, {name: name}, { headers: headers });
  }

  deleteDocument(id: string): Observable<any> | null {
    var token = this.tokenService.getToken();
    if (!token)
      return null;
    var headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.delete(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }
}
