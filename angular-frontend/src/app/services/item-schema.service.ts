import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ItemSchema } from "@models/item-schema";
import { GenericRepositoryService } from "./generic-repository.service";
import { environment } from "@env";
import { UserService } from "@services/user.service";

@Injectable({
  providedIn: "root"
})
export class ItemSchemaService extends GenericRepositoryService<ItemSchema> {

  constructor(httpClient: HttpClient, userService: UserService) {
    super(httpClient, userService);
  }

  get entitiesUrl() {
    return `${environment.apiUrl}/itemSchema`;
  }
}
