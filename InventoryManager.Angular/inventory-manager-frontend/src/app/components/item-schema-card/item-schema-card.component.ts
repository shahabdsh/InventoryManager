import {Component, Input, OnInit} from '@angular/core';
import {ItemSchema, ItemSchemaPropertyType} from '../../models/item-schema';
import {FormArray, FormBuilder, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-item-schema-card',
  templateUrl: './item-schema-card.component.html',
  styleUrls: ['./item-schema-card.component.scss']
})
export class ItemSchemaCardComponent implements OnInit {

  @Input() schema: ItemSchema;
  schemaForm: FormGroup;

  // Todo: Automatically init this.
  itemPropertyTypes = [
    { label: 'Text', value: ItemSchemaPropertyType.Text },
    { label: 'Number', value: ItemSchemaPropertyType.Number },
  ];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {

    const properties = this.schema.properties.map(prop => {
      return this.fb.group({
        name: prop.name,
        type: prop.type
      });
    });

    this.schemaForm = this.fb.group({
      name: [this.schema.name],
      properties: this.fb.array(properties),
    });
  }

  get properties(): FormArray {
    return this.schemaForm.get('properties') as FormArray;
  }

  getTypeLabel (typeValue) {
    return this.itemPropertyTypes.find(prop => prop.value === typeValue).label;
  }
}
