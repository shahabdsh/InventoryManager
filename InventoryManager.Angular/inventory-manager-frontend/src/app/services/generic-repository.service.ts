import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {filter, shareReplay, take} from "rxjs/operators";
import {EntityBase} from "../models/entity-base";

export abstract class GenericRepositoryService <T extends EntityBase> {

  allItems$: BehaviorSubject<T[]>;

  protected constructor(protected httpClient: HttpClient) {

    this.allItems$ = new BehaviorSubject<T[]>(null);
  }

  get allItems () {
    return this.allItems$.pipe(filter(val => val != undefined));
  }

  get allItemsTakeOne () {
    return this.allItems.pipe(take(1));
  }

  abstract get itemsUrl(): string;

  getAll(): Observable<T[]> {

    let ob = this.httpClient.get<T[]>(this.itemsUrl).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems$.next(result);
    });

    return ob;
  }

  getAllIfCurrentItemsAreNull () {
    this.allItems$.pipe(take(1)).subscribe(current => {
      if (!current) {
        this.getAll();
      }
    });
  }

  update(id: string, item: T) {

    let ob = this.httpClient.put(`${this.itemsUrl}/${id}`, item).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems.pipe(take(1)).subscribe(current => {

        let currentItem = current.find(x => x.id === id);

        for (const property in currentItem) {
          if (currentItem.hasOwnProperty(property)) {
            currentItem[property] = item[property];
          }
        }

        this.allItems$.next(current);
      })
    });

    return ob;
  }

  delete(id: string) {

    let ob = this.httpClient.delete(`${this.itemsUrl}/${id}`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems.pipe(take(1)).subscribe(current => {
        let newArray = current.filter(x => x.id !== id);
        this.allItems$.next(newArray);
      })
    });

    return ob;
  }

  create(item: T): Observable<T> {

    let ob = this.httpClient.post<T>(`${this.itemsUrl}`, item).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allItems.pipe(take(1)).subscribe(current => {
        current.push(result);
        this.allItems$.next(current);
      })
    });

    return ob;
  }
}
