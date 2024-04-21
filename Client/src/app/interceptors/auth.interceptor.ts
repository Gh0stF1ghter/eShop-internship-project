import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable, catchError, map, tap, throwError } from 'rxjs';
import userTokens from '../models/userTokens';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.authService.getAccessToken();

    if (accessToken) {
      req = this.setAuthorizationHeader(req, accessToken);
    }

    return next.handle(req).pipe(
      catchError((error) => {
        if (!this.authService.tokenRefreshing) {
          if (error.status == 401) {
            if (accessToken) {
              this.tryRefresh(req, next);
            } else {
              this.goToSignIn();
            }
          } else if (error.status == 403) {
            console.log('Not permitted to perform the request');
          }
        }

        return throwError(() => error);
      })
    );
  }

  tryRefresh(req: HttpRequest<any>, next: HttpHandler) {
    this.authService.refreshToken().subscribe({
      next: (userTokens: userTokens) => {
        this.authService.SetSession(userTokens);

        req = this.setAuthorizationHeader(req, userTokens.accessToken);

        return next.handle(req);
      },
      error: (err) => {
        this.authService.logOut();
        console.log(err);
        this.goToSignIn();
      },
    });

    return throwError(
      () => new Error('Something went wrong during the authorization')
    );
  }

  setAuthorizationHeader(
    req: HttpRequest<any>,
    accessToken: string
  ): HttpRequest<any> {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });

    return req;
  }

  private goToSignIn() {
    this.router.navigate(['/login']);
  }
}
