import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ItemSchema} from '../models/item-schema';
import {GenericRepositoryService} from "./generic-repository.service";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ItemSchemaService extends GenericRepositoryService<ItemSchema> {

  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  get entitiesUrl() {
    return `${environment.apiUrl}/itemSchema`;
  }
}
