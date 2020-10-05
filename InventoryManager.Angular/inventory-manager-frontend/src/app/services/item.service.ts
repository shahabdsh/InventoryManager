import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import { Item } from '../models/item';
import {ItemSchema} from "../models/item-schema";
import {shareReplay, take} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class ItemService {

  itemsUrl = 'https://localhost:5001/api/item';

  allItems$: BehaviorSubject<Item[]>;

  constructor(private httpClient: HttpClient) {
    // Todo: Preferably, this should only be called when needed.

    this.allItems$ = new BehaviorSubject<Item[]>([]);

    this.getAll().subscribe(items => {
      this.allItems$.next(items);
    });
  }

  getAll(): Observable<Item[]> {
    return this.httpClient.get<Item[]>(this.itemsUrl);
  }

  update(id: string, item: Item) {
    return this.httpClient.put(`${this.itemsUrl}/${id}`, item);
  }

  delete(id: string) {
    let ob = this.httpClient.delete(`${this.itemsUrl}/${id}`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems$.pipe(take(1)).subscribe(current => {
        let newArray = current.filter(x => x.id !== id);
        this.allItems$.next(newArray);
      })
    });

    return ob;
  }

  createUsingSchema(schema: ItemSchema): Observable<Item> {

    let item = new Item({
      name: "New Item",
      type: schema.name,
      quantity: 0,
    });

    let ob = this.httpClient.post<Item>(`${this.itemsUrl}`, item).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems$.pipe(take(1)).subscribe(current => {
        current.push(result);
        this.allItems$.next(current);
      })
    });

    return ob;
  }
}
