import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Item } from '../models/item';
import {ItemSchema} from "../models/item-schema";

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  itemsUrl = 'https://localhost:5001/api/item';

  constructor(private httpClient: HttpClient) { }

  getAllItems(): Observable<Item[]> {
    return this.httpClient.get<Item[]>(this.itemsUrl);
  }

  updateItem(id: string, item: Item) {
    return this.httpClient.put(`${this.itemsUrl}/${id}`, item);
  }

  createUsingSchema(schema: ItemSchema): Observable<Item> {

    let item = new Item({
      name: "New Item",
      type: schema.name,
      quantity: 0,
    });

    return this.httpClient.post<Item>(`${this.itemsUrl}`, item);
  }
}
