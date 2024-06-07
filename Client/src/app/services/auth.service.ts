import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import userTokens from '../models/userTokens';
import { identityEndpoints } from '../constants/environment';
import moment from 'moment';
import { firstValueFrom, tap } from 'rxjs';
import signUp from '../models/signupModel';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  tokenRefreshing = false;

  async register(userCredentials: signUp) {
      return this.http.post(identityEndpoints.register, userCredentials)
  }

  async login(email: string, password: string) {
    const tokens = await firstValueFrom(
      this.http.post<userTokens>(identityEndpoints.login, { email, password })
    ).catch(() => {});

    if (tokens) {
      this.SetSession(tokens);
      return true;
    }

    return false;
  }

  SetSession(tokens: userTokens) {
    const dataFromToken = this.getFromToken(tokens.accessToken);

    const expiresAt = dataFromToken.exp * 1000

    localStorage.setItem('access_token', tokens.accessToken);
    localStorage.setItem('refresh_token', tokens.refreshToken);
    localStorage.setItem('token_expires', JSON.stringify(expiresAt));
  }

  isLogged() {
    const expiration = localStorage.getItem('token_expires');

    if (expiration) {
      const expiresAt = JSON.parse(expiration);

      const currentTime = Date.now()
      return currentTime <= expiresAt;
    }

    return false;
  }

  getAccessToken() {
    return localStorage.getItem('access_token');
  }
  getRefreshToken() {
    return localStorage.getItem('refresh_token');
  }

  refreshToken() {
    let accessToken = this.getAccessToken();
    let refreshToken = this.getRefreshToken();

    this.tokenRefreshing = true

    return this.http.post<userTokens>(identityEndpoints.refreshToken, {
      accessToken,
      refreshToken
    }).pipe(tap(() => {
      this.tokenRefreshing = false
    }))
  }

  getFromToken(token: string) {
    return JSON.parse(window.atob(token.split('.')[1]));
  }

  logOut() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('token_expires');
  }

  getUserRole() {
    const accessToken = this.getAccessToken();

    if (accessToken) {
      const { role } = this.getFromToken(accessToken) as { role: string };

      return role;
    }

    return null;
  }

  getUserId() {
    const accessToken = this.getAccessToken();

    if (accessToken) {
      const { unique_name } = this.getFromToken(accessToken) as { unique_name: string };

      return unique_name;
    }

    return null;
  }
}
