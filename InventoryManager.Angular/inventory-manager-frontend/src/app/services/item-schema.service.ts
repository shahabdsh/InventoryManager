import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../models/item';
import {ItemSchema} from '../models/item-schema';

@Injectable({
  providedIn: 'root'
})
export class ItemSchemaService {

  itemSchemasUrl = 'https://localhost:5001/api/itemschema';

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<ItemSchema[]> {
    return this.httpClient.get<Item[]>(this.itemSchemasUrl);
  }

  update(id: string, itemSchema: ItemSchema) {
    return this.httpClient.put(`${this.itemSchemasUrl}/${id}`, itemSchema);
  }
}
