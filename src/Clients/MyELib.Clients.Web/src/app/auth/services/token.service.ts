import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private storageKey = 'token';

  setToken(token: string) {
    localStorage.setItem(this.storageKey, token);
  }

  getToken(): string | null {
    const token = localStorage.getItem(this.storageKey);
    if (token) {
      const tokenPayload = this.parseToken(token);
      if (tokenPayload.exp < Date.now() / 1000) {
        this.removeToken();
        return null;
      }
    }
    return token;
  }

  removeToken() {
    localStorage.removeItem(this.storageKey);
  }

  private parseToken(token: string): any {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace('-', '+').replace('_', '/');
    return JSON.parse(window.atob(base64));
  }
}
