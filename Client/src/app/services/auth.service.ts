import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import userTokens from '../models/userTokens';
import { identityEndpoints } from '../constants/environment';
import moment from 'moment';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) {}

  async login(email: string, password: string) {
    const tokens = await firstValueFrom(this.http
      .post<userTokens>(identityEndpoints.login, { email, password })).catch(() => {});

      if (tokens) {
        this.SetSession(tokens)
        return true
      }

    return false
  }

  SetSession(tokens: userTokens) {
    const dataFromToken = this.getFromToken(tokens.accessToken);

    const expires = moment().add(dataFromToken.exp);

    localStorage.setItem('access_token', tokens.accessToken);
    localStorage.setItem('refresh_token', tokens.refreshToken);
    localStorage.setItem('token_expires', JSON.stringify(expires.valueOf()));
  }

  isLogged() {
    const expiration = localStorage.getItem('token_expires');

    if (expiration) {
      const expiresAt = JSON.parse(expiration);
      return moment().isBefore(expiresAt);
    }

    return false;
  }

  getAccessToken() {
    return localStorage.getItem('access_token')
  }
  getRefreshToken() {
    return localStorage.getItem('refresh_token')
  }

  refreshAccessToken() {
    let accessToken = this.getAccessToken()
    let refreshToken = this.getRefreshToken()

    
  }

  getFromToken(token: string) {
    return JSON.parse(window.atob(token.split('.')[1]));
  }

  logOut() {
    localStorage.removeItem('access_token')
    localStorage.removeItem('refresh_token')
    localStorage.removeItem('token_expires');
  }
}
