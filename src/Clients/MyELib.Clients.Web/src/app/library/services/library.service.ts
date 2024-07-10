import { Injectable } from '@angular/core';
import { LibraryModel } from '../models/library.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { TokenService } from '../../auth/services/token.service';
import { tap } from 'rxjs/internal/operators/tap';
import { map } from 'rxjs/internal/operators/map';
import { PaginationData } from '../pagination/pagination.data';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {
  private url = "Library";

  paginationData: PaginationData = {
    currentPage: 0,
    pageCount: 0,
    pageSize: 0,
    totalCount: 0,
  }

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getLibraries(pageNumber: number, pageSize: number): Observable<LibraryModel[]> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.get<LibraryModel[]>(`${environment.apiUrl}/${this.url}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      observe: 'response',
      headers: headers
    }).pipe(
      tap((response) => {
        this.paginationData.pageCount = +response.headers.get("X-Page-Count")!;
        this.paginationData.currentPage = +response.headers.get('X-Current-Page')!;
        this.paginationData.pageSize = +response.headers.get('X-Page-Size')!;
        this.paginationData.totalCount = +response.headers.get('X-Total-Count')!;
      }),
      map((response) => response.body!)
    );
  }

  getPaginationData(): PaginationData {
    return this.paginationData;
  }

  getLibraryById(id: string): Observable<LibraryModel> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.get<LibraryModel>(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }

  postLibrary(library: LibraryModel): Observable<string> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http
      .post<string>(`${environment.apiUrl}/${this.url}`, { name: library.name, documentIds: library.documents.map<string>(d => d.id) }, { headers: headers });
  }

  putLibrary(library: LibraryModel): Observable<any> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    let updateLibrary = {
      name: library.name,
      documentIds: library.documents.map<string>(d => d.id)
    }
    return this.http.put(`${environment.apiUrl}/${this.url}/${library.id}`, updateLibrary , { headers: headers });
  }

  deleteLibraryById(id: string) : Observable<any> | null {
    let token = this.tokenService.getToken();
    if (!token)
      return null;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);
    return this.http.delete<LibraryModel>(`${environment.apiUrl}/${this.url}/${id}`, { headers: headers });
  }
}
