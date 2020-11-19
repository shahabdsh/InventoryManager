import { Component } from "@angular/core";
import { UserService } from "@services/user.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent {
  title = "inventory-manager-frontend";

  constructor(public userService: UserService) {
  }

  logout () {
    this.userService.logout(true).subscribe();
  }
}
