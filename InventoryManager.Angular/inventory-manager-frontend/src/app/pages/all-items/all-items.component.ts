import { Component, OnInit } from '@angular/core';
import { ItemService } from '../../services/item.service';
import {BehaviorSubject, Observable, Subject} from 'rxjs';
import {Item} from '../../models/item';
import {ItemSchemaService} from "../../services/item-schema.service";
import {ItemSchema} from "../../models/item-schema";
import {take} from "rxjs/operators";

@Component({
  selector: 'app-all-items',
  templateUrl: './all-items.component.html',
  styleUrls: ['./all-items.component.scss']
})
export class AllItemsComponent implements OnInit {

  schemas: BehaviorSubject<ItemSchema[]>;

  constructor(public itemsService: ItemService, private itemSchemaService: ItemSchemaService) { }

  ngOnInit(): void {

    this.schemas = new BehaviorSubject<ItemSchema[]>([]);

    this.itemSchemaService.allSchemas$.subscribe(result => {
      this.schemas.next(result);
    });
  }

  addItem (schema: ItemSchema) {
    this.itemsService.createUsingSchema(schema).subscribe();
  }
}
