import { Component, OnInit } from '@angular/core';
import { ItemService } from '../../services/item.service';
import {ItemSchemaService} from "../../services/item-schema.service";
import {ItemSchema} from "../../models/item-schema";
import {Item} from "../../models/item";

@Component({
  selector: 'app-all-items',
  templateUrl: './all-items.component.html',
  styleUrls: ['./all-items.component.scss']
})
export class AllItemsComponent implements OnInit {

  constructor(public itemsService: ItemService, private itemSchemaService: ItemSchemaService) { }

  ngOnInit(): void {
    this.itemsService.getAllIfCurrentEntitiesAreNull();
    this.itemSchemaService.getAllIfCurrentEntitiesAreNull();
  }

  addItem (schema: ItemSchema) {
    this.itemsService.createUsingSchema(schema).subscribe();
  }

  get schemas$ () {
    return this.itemSchemaService.allEntities$;
  }

  itemTrackBy (index: number, item: Item) {
    return item.id;
  }
}
