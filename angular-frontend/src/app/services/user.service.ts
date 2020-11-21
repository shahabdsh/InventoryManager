import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { EMPTY, Observable, Subject } from "rxjs";
import { shareReplay } from "rxjs/operators";
import { environment } from "@env";
import { LoginResponse } from "@models/login-response";
import { Router } from "@angular/router";
import { AuthEvent } from "@models/auth-event";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  static readonly LOCAL_STORAGE_KEY = "user";

  authEvents$: Subject<AuthEvent> = new Subject<AuthEvent>();

  loginResponse: LoginResponse;

  constructor(private httpClient: HttpClient, private router: Router) {
    this.loginResponse = JSON.parse(localStorage.getItem(UserService.LOCAL_STORAGE_KEY));
  }

  loginGuest(): Observable<LoginResponse> {

    const ob = this.httpClient.get<LoginResponse>(`${this.url}/guest`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.saveLoginResponse(result);
      this.authEvents$.next(AuthEvent.LoggedIn);
    });

    return ob;
  }

  loginGoogle(idToken: string): Observable<LoginResponse> {

    const ob = this.httpClient.post<LoginResponse>(`${this.url}/google`,
      {
        token: idToken
      }).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.saveLoginResponse(result);
      this.authEvents$.next(AuthEvent.LoggedIn);
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
        this.clearLoginResponse();
        this.authEvents$.next(AuthEvent.LoggedOut);
      });

      return ob
    } else {
      this.clearLoginResponse();
      this.authEvents$.next(AuthEvent.LoggedOut);
      this.router.navigate(['login']);
      return EMPTY;
    }
  }

  saveLoginResponse(loginResponse: LoginResponse) {
    localStorage.setItem(UserService.LOCAL_STORAGE_KEY, JSON.stringify(loginResponse));
    this.loginResponse = loginResponse;
  }

  clearLoginResponse () {
    this.loginResponse = null;
    localStorage.removeItem(UserService.LOCAL_STORAGE_KEY);
  }

  get pictureUrl () {
    let url = this.loginResponse?.profileImageUrl;

    return url ?? environment.defaultProfilePicUrl;
  }

  get token () {
    return this.loginResponse?.token;
  }

  get url() {
    return `${environment.apiUrl}/user`;
  }
}
