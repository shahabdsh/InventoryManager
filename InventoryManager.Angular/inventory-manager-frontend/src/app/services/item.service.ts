import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item } from '../models/item';
import { ItemSchema } from "../models/item-schema";
import { GenericRepositoryService } from "./generic-repository.service";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ItemService extends GenericRepositoryService<Item> {

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  get entitiesUrl() {
    return `${environment.apiUrl}/item`;
  }

  createUsingSchema(schema: ItemSchema): Observable<Item> {

    let item = new Item({
      name: "New Item",
      schemaId: schema.id,
      quantity: 0,
    });

    return this.create(item);
  }
}
