import { Component, OnInit } from '@angular/core';
import { UserService } from "@services/user.service";
import { Router } from "@angular/router";
import { GoogleLoginProvider, SocialAuthService } from "angularx-social-login";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private userService: UserService,
              private router: Router,
              private authService: SocialAuthService) { }

  ngOnInit(): void {
  }

  guest () {
    this.userService.loginGuest().subscribe(() => {
      this.router.navigate(['all-items']);
    });
  }

  google () {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(val => {
      this.userService.loginGoogle(val.idToken).subscribe(() => {
        this.router.navigate(['all-items']);
      });
    });
  }
}
