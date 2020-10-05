import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../models/item';
import {ItemSchema} from '../models/item-schema';
import {shareReplay} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ItemSchemaService {

  itemSchemasUrl = 'https://localhost:5001/api/itemschema';

  allSchemas$: Observable<ItemSchema[]>;

  constructor(private httpClient: HttpClient) {
    this.refreshSchemasCache();
  }

  refreshSchemasCache () {
    this.allSchemas$ = this.getAll().pipe(
      shareReplay(1)
    );
  }

  getAll(): Observable<ItemSchema[]> {
    return this.httpClient.get<ItemSchema[]>(this.itemSchemasUrl);
  }

  update(id: string, itemSchema: ItemSchema) {

    let ob = this.httpClient.put(`${this.itemSchemasUrl}/${id}`, itemSchema).pipe(
      shareReplay(1)
    );

    ob.subscribe(() => {
      this.refreshSchemasCache();
    });

    return ob;
  }
}
