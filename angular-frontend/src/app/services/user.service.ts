import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { EMPTY, Observable, Subject } from "rxjs";
import { shareReplay } from "rxjs/operators";
import { environment } from "@env";
import { TokenResponse } from "@models/token-response";
import { Router } from "@angular/router";
import { AuthEvent } from "@models/auth-event";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  static readonly TOKEN_KEY = "token";

  authStatus$: Subject<AuthEvent> = new Subject<AuthEvent>();

  constructor(private httpClient: HttpClient, private router: Router) { }

  loginGuest(): Observable<TokenResponse> {

    const ob = this.httpClient.get<TokenResponse>(`${this.url}/guest`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      localStorage.setItem(UserService.TOKEN_KEY, result.token);
      this.authStatus$.next(AuthEvent.LoggedIn);
    });

    return ob;
  }

  loginGoogle(idToken: string): Observable<TokenResponse> {

    const ob = this.httpClient.post<TokenResponse>(`${this.url}/google`,
      {
        token: idToken
      }).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      localStorage.setItem(UserService.TOKEN_KEY, result.token);
      this.authStatus$.next(AuthEvent.LoggedIn);
    });

    return ob;
  }

  logout (revokeToken = false): Observable<void> {

    if (revokeToken) {
      const ob = this.httpClient.get<void>(`${this.url}/logout`)
        .pipe(shareReplay(1));

      ob.subscribe(() => {
        this.router.navigate(['login']);
      }, () => {

      }, () => {
        localStorage.removeItem(UserService.TOKEN_KEY);
        this.authStatus$.next(AuthEvent.LoggedOut);
      });

      return ob
    } else {
      localStorage.removeItem(UserService.TOKEN_KEY);
      this.authStatus$.next(AuthEvent.LoggedOut);
      this.router.navigate(['login']);
      return EMPTY;
    }
  }

  get token () {
    return localStorage.getItem(UserService.TOKEN_KEY);
  }

  get url() {
    return `${environment.apiUrl}/user`;
  }
}
