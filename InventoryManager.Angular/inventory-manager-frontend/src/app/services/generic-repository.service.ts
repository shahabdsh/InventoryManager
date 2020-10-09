import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {filter, shareReplay, take} from "rxjs/operators";
import {EntityBase} from "../models/entity-base";

export abstract class GenericRepositoryService <T extends EntityBase> {

  allEntities$: BehaviorSubject<T[]>;

  protected constructor(protected httpClient: HttpClient) {

    this.allEntities$ = new BehaviorSubject<T[]>(null);
  }

  get allEntities () {
    return this.allEntities$.pipe(filter(val => val != undefined));
  }

  get allEntitiesTakeOne () {
    return this.allEntities.pipe(take(1));
  }

  abstract get entitiesUrl(): string;

  getAll(): Observable<T[]> {

    let ob = this.httpClient.get<T[]>(this.entitiesUrl).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allEntities$.next(result);
    });

    return ob;
  }

  getAllIfCurrentEntitiesAreNull () {
    this.allEntities$.pipe(take(1)).subscribe(current => {
      if (!current) {
        this.getAll();
      }
    });
  }

  update(id: string, entity: T) {

    let ob = this.httpClient.put(`${this.entitiesUrl}/${id}`, entity).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allEntities.pipe(take(1)).subscribe(current => {

        let currentEntity = current.find(x => x.id === id);

        for (const property in currentEntity) {
          if (currentEntity.hasOwnProperty(property)) {
            currentEntity[property] = entity[property];
          }
        }

        this.allEntities$.next(current);
      })
    });

    return ob;
  }

  delete(id: string) {

    let ob = this.httpClient.delete(`${this.entitiesUrl}/${id}`).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allEntities.pipe(take(1)).subscribe(current => {
        let newArray = current.filter(x => x.id !== id);
        this.allEntities$.next(newArray);
      })
    });

    return ob;
  }

  create(entity: T): Observable<T> {

    let ob = this.httpClient.post<T>(`${this.entitiesUrl}`, entity).pipe(
      shareReplay(1)
    );

    ob.subscribe(result => {
      this.allEntities.pipe(take(1)).subscribe(current => {
        current.push(result);
        this.allEntities$.next(current);
      })
    });

    return ob;
  }
}
