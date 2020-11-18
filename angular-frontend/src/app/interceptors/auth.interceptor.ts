import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from "@angular/router";
import { shareReplay } from "rxjs/operators";
import { UserService } from "@services/user.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router, private userService: UserService) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let token = this.userService.token;

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    let ob = next.handle(req).pipe(
      shareReplay(1)
    );

    ob.subscribe(() => {}, err => {
      console.log(err);
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          this.userService.logout()
        }
      }
    });

    return ob;
  }
}
