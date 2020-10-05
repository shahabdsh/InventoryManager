import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../models/item';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ItemService} from '../../services/item.service';
import {debounceTime} from 'rxjs/operators';
import {DatePipe} from '@angular/common';
import {ItemSchemaService} from '../../services/item-schema.service';
import {ItemSchema, ItemSchemaProperty, ItemSchemaPropertyType} from '../../models/item-schema';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {

  @Input() item: Item;
  itemForm: FormGroup;
  schema: ItemSchema;

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
      type: [this.item.type],
      quantity: [this.item.quantity]
    });

    this.allProperties = [];

    this.itemSchemaService.allSchemas$.subscribe(schemas => {
      this.schema = schemas.find(s => s.name === this.item.type);
      const config = {};
      this.schema.properties.forEach(property => {
        config[property.name] = [this.item.properties ? this.item.properties[property.name] : null];
        this.allProperties.push({
          name: property.name, type: property.type
        });
      });

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

  registerAutosave() {
    this.itemForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(_ => {

        const obj = new Item(this.itemForm.value) as any;
        obj.id = this.item.id;

        this.itemsService.updateItem(this.item.id, obj).subscribe(response => {
          this.footerMessage = `Updated on: ${this.datePipe.transform(new Date(), 'medium')}`;
        });
      });
  }
}
