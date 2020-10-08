import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item } from '../models/item';
import { ItemSchema } from "../models/item-schema";
import { GenericRepositoryService } from "./generic-repository.service";

@Injectable({
  providedIn: 'root'
})
export class ItemService extends GenericRepositoryService<Item> {

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  get itemsUrl() {
    return 'https://localhost:5001/api/item';
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
