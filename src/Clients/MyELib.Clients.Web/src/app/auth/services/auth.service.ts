import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { TokenService } from './token.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = "Auth";
  private loginUrl = "login";
  private registerUrl = "register";

  constructor(private http: HttpClient, private tokenService: TokenService, private router: Router) { }

  login(username: string, password: string) {
    if (this.tokenService.getToken()) {
      return;
    }

    const credentials = { username, password };

    this.http.post(`${environment.apiUrl}/${this.url}/${this.loginUrl}`, credentials, { responseType: 'text' })
      .subscribe((token: string) => {
        this.tokenService.setToken(token)
        this.router.navigateByUrl("").then(() => window.location.reload());
      });
  }

  register(email: string, username: string, password: string) {
    if (this.tokenService.getToken()) {
      return;
    }

    let credentials = { username, email, password };

    this.http.post(`${environment.apiUrl}/${this.url}/${this.registerUrl}`, credentials).subscribe();
  }

  logout() {
    if (!this.tokenService.getToken()) {
      return;
    }

    this.tokenService.setToken("");
    this.router.navigateByUrl("login");
  }
}
