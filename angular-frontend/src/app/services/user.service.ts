import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { shareReplay } from "rxjs/operators";
import { environment } from "@env";
import { TokenResponse } from "@models/token-response";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  static readonly TOKEN_KEY = "token";

  constructor(private httpClient: HttpClient, private router: Router) { }

  loginGuest(): Observable<TokenResponse> {

    const ob = this.httpClient.get<TokenResponse>(`${this.url}/guest`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      localStorage.setItem(UserService.TOKEN_KEY, result.token);
    });

    return ob;
  }

  logout () {

    this.router.navigate(['login']);

    localStorage.removeItem(UserService.TOKEN_KEY);

    // Todo: revoke token in backend.
  }

  get token () {
    return localStorage.getItem(UserService.TOKEN_KEY);
  }

  get url() {
    return `${environment.apiUrl}/user`;
  }
}
