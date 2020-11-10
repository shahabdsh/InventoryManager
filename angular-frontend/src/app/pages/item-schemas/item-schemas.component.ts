import { Component, OnInit } from "@angular/core";
import { ItemSchemaService } from "@services/item-schema.service";
import { ItemSchema } from "@models/item-schema";

@Component({
  selector: "app-item-schemas",
  templateUrl: "./item-schemas.component.html",
  styleUrls: ["./item-schemas.component.scss"]
})
export class ItemSchemasComponent implements OnInit {

  constructor(private itemSchemaService: ItemSchemaService) {
  }

  ngOnInit(): void {
    this.itemSchemaService.getAllIfCurrentEntitiesAreNull();
  }

  get schemas$() {
    return this.itemSchemaService.allEntities$;
  }

  add() {

    let name = "New Schema";
    this.itemSchemaService.allEntitiesTakeOne.subscribe(schemas => {

      let n = 2;
      while (schemas.some(schema => {
        return schema.name === name;
      })) {
        name = "New Schema " + n;
        n++;
      }
    });

    const obj = {
      name, properties: []
    } as ItemSchema;

    this.itemSchemaService.create(obj).subscribe();
  }

  itemTrackBy(index: number, itemSchema: ItemSchema) {
    return itemSchema.id;
  }
}
