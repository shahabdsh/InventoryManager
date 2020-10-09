import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../models/item';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ItemService} from '../../services/item.service';
import {debounceTime} from 'rxjs/operators';
import {DatePipe} from '@angular/common';
import {ItemSchemaService} from '../../services/item-schema.service';
import {ItemSchema, ItemSchemaProperty, ItemSchemaPropertyType} from '../../models/item-schema';
import {timer} from "rxjs";

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {

  @Input() item: Item;
  itemForm: FormGroup;
  schema: ItemSchema;

  showConfirmDelete: boolean

  footerMessage: string;

  allProperties: ItemSchemaProperty[];

  constructor(private fb: FormBuilder,
              private itemsService: ItemService,
              private datePipe: DatePipe,
              private itemSchemaService: ItemSchemaService) {
  }

  ngOnInit(): void {
    this.itemForm = this.fb.group({
      name: [this.item.name],
      quantity: [this.item.quantity]
    });

    this.allProperties = [];

    this.itemSchemaService.allEntitiesTakeOne.subscribe(schemas => {

      this.schema = schemas.find(s => s.id === this.item.schemaId);
      const config = {};

      if (this.schema) {
        this.schema.properties.forEach(property => {
          config[property.name] = [this.item.properties ? this.item.properties[property.name] : null];
          this.allProperties.push({
            name: property.name, type: property.type
          });
        });
      }

      let existingProperties = this.item.properties;

      for (const property in existingProperties) {
        if (existingProperties.hasOwnProperty(property) && existingProperties[property] && !config.hasOwnProperty(property)) {
          config[property] = [this.item.properties ? this.item.properties[property] : null];
          this.allProperties.push({
            name: property, type: ItemSchemaPropertyType.Text
          });
        }
      }

      this.itemForm.addControl('properties', this.fb.group(config));

      this.registerAutosave();
    });

    if (this.item.updatedOn) {
      this.footerMessage = `Updated on: ${this.datePipe.transform(new Date(), 'medium')}`;
    } else if (this.item.createdOn) {
      this.footerMessage = `Created on: ${this.datePipe.transform(new Date(), 'medium')}`;
    }
  }

  attemptDelete () {
    this.showConfirmDelete = true;

    timer(2000).subscribe(() => {
      this.showConfirmDelete = false;
    });
  }

  delete () {
    this.itemsService.delete(this.item.id).subscribe();
  }

  registerAutosave() {
    this.itemForm.valueChanges
      .pipe(debounceTime(600))
      .subscribe(_ => {

        const obj = new Item(this.itemForm.getRawValue()) as Item;
        obj.id = this.item.id;
        obj.schemaId = this.item.schemaId;

        this.itemsService.update(this.item.id, obj).subscribe(response => {
          this.footerMessage = "Saved!";
          timer(1500).subscribe(() => {
            this.footerMessage = `Updated on: ${this.datePipe.transform(new Date(), 'medium')}`;
          });
        });
      });
  }

  get itemType () {
    return this.schema ? this.schema.name : "Untyped";
  }
}
