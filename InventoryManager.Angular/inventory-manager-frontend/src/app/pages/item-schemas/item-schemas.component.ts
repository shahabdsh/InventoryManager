import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Item } from '../../models/item';
import {ItemSchemaService} from '../../services/item-schema.service';
import {ItemSchema} from '../../models/item-schema';

@Component({
  selector: 'app-item-schemas',
  templateUrl: './item-schemas.component.html',
  styleUrls: ['./item-schemas.component.scss']
})
export class ItemSchemasComponent implements OnInit {

  constructor(private itemSchemaService: ItemSchemaService) { }

  ngOnInit(): void {
    this.itemSchemaService.getAllIfCurrentItemsAreNull();
  }

  get schemas$ () {
    return this.itemSchemaService.allItems$;
  }

  add () {

    let name = "New Schema"
    this.itemSchemaService.allItemsTakeOne.subscribe(schemas => {

      let number = 2;
      while (schemas.some(schema => {
        return schema.name === name;
      })) {
        name = "New Schema " + number;
        number++;
      }
    })

    let obj = {
      name: name, properties: []
    } as ItemSchema;

    this.itemSchemaService.create(obj).subscribe();
  }

  itemTrackBy (index: number, itemSchema: ItemSchema) {
    return itemSchema.id;
  }
}
