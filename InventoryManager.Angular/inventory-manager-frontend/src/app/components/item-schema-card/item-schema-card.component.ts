import {Component, Input, OnInit} from '@angular/core';
import {ItemSchema} from '../../models/item-schema';
import {FormArray, FormBuilder, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-item-schema-card',
  templateUrl: './item-schema-card.component.html',
  styleUrls: ['./item-schema-card.component.scss']
})
export class ItemSchemaCardComponent implements OnInit {

  @Input() schema: ItemSchema;
  schemaForm: FormGroup;

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
}
