import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Item } from "../models/item";

@Injectable({
  providedIn: 'root'
})
export class ItemsService {

  itemsUrl = 'https://localhost:5001/api/item';

  constructor(private httpClient: HttpClient) { }

  getAllItems (): Observable<Item[]> {
    return this.httpClient.get<Item[]>(this.itemsUrl);
  }

  updateItem (id: string, item: Item) {
    return this.httpClient.put(`${this.itemsUrl}/${id}`, item);
  }
}
