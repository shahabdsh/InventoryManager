import {Component, Input, OnInit} from '@angular/core';
import {ItemSchema, ItemSchemaPropertyType} from '../../models/item-schema';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ItemSchemaService} from '../../services/item-schema.service';
import {Item} from '../../models/item';

@Component({
  selector: 'app-item-schema-card',
  templateUrl: './item-schema-card.component.html',
  styleUrls: ['./item-schema-card.component.scss']
})
export class ItemSchemaCardComponent implements OnInit {

  @Input() schema: ItemSchema;
  schemaForm: FormGroup;

  changed: boolean;

  // Todo: Automatically init this.
  itemPropertyTypes = [
    { label: 'Text', value: ItemSchemaPropertyType.Text },
    { label: 'Number', value: ItemSchemaPropertyType.Number },
  ];

  constructor(private fb: FormBuilder, private itemSchemaService: ItemSchemaService) { }

  ngOnInit(): void {

    const properties = this.schema.properties.map(prop => {
      return this.fb.group({
        name: [prop.name, Validators.required],
        type: prop.type
      });
    });

    this.schemaForm = this.fb.group({
      name: [this.schema.name],
      properties: this.fb.array(properties),
    });

    this.schemaForm.valueChanges.subscribe(_ => {
      this.changed = true;
    });

    this.changed = false;
  }

  createNewProperty(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      type: ItemSchemaPropertyType.Text,
    });
  }

  addNewPropertyToForm() {
    this.properties.push(this.createNewProperty());
  }

  removeProperty(i: number) {
    this.properties.removeAt(i);
  }

  get properties(): FormArray {
    return this.schemaForm.get('properties') as FormArray;
  }

  getTypeLabel(typeValue) {
    return this.itemPropertyTypes.find(prop => prop.value === typeValue).label;
  }

  save() {

    if (!this.schemaForm.valid) {
      return;
    }

    const obj = new Item(this.schemaForm.value) as any;
    obj.id = this.schema.id;

    console.log(this.schemaForm.value);

    this.itemSchemaService.update(this.schema.id, obj).subscribe();

    this.changed = false;
  }
}
