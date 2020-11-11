import { Component, Input, OnInit } from "@angular/core";
import { Item } from "@models/item";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ItemService } from "@services/item.service";
import { debounceTime } from "rxjs/operators";
import { DatePipe } from "@angular/common";
import { ItemSchemaService } from "@services/item-schema.service";
import { ItemSchema, ItemSchemaPropertyType } from "@models/item-schema";
import { timer } from "rxjs";
import { applyValidationErrorsToFormGroup } from "@utils/apply-validation-errors-to-form-group";
import { checkFormInvalidAndSignalChange } from "@utils/check-form-invalid-and-signal-change";

@Component({
  selector: "app-item-card",
  templateUrl: "./item-card.component.html",
  styleUrls: ["./item-card.component.scss"]
})
export class ItemCardComponent implements OnInit {

  @Input() item: Item;
  itemForm: FormGroup;
  schema: ItemSchema;

  showConfirmDelete: boolean;
  footerMessage: string;

  constructor(private fb: FormBuilder,
              private itemsService: ItemService,
              private datePipe: DatePipe,
              private itemSchemaService: ItemSchemaService) {
  }

  ngOnInit(): void {
    this.itemForm = this.fb.group({
      name: [this.item.name, Validators.required],
      quantity: [this.item.quantity, Validators.required]
    });

    this.itemSchemaService.allEntitiesTakeOne.subscribe(schemas => {

      this.schema = schemas.find(s => s.id === this.item.schemaId);

      let propertiesConfig = [];

      this.schema.properties.forEach(schemaProp => {
        let itemProp = this.item.properties.find(p => p.key === schemaProp.name);

        if (itemProp) {
          propertiesConfig.push(this.fb.group({
            key: [schemaProp.name],
            value: [schemaProp.type === ItemSchemaPropertyType.Checkbox ? (itemProp.value === "true") : itemProp.value]
          }));
        } else {
          propertiesConfig.push(this.fb.group({
            key: [schemaProp.name],
            value: [schemaProp.type === ItemSchemaPropertyType.Checkbox ? false : ""]
          }));
        }
      });

      this.item.properties.forEach(property => {
        if (property.value && this.schema.properties.find(p => p.name === property.key) === undefined) {
          propertiesConfig.push(this.fb.group({
            key: property.key,
            value: property.value
          }));
        }
      });

      this.itemForm.addControl("properties", this.fb.array(propertiesConfig));

      this.registerAutosave();
    });

    if (this.item.updatedOn) {
      this.footerMessage = `Updated on: ${this.datePipe.transform(this.item.updatedOn, "medium")}`;
    } else if (this.item.createdOn) {
      this.footerMessage = `Created on: ${this.datePipe.transform(this.item.createdOn, "medium")}`;
    }
  }

  attemptDelete() {
    this.showConfirmDelete = true;

    timer(2000).subscribe(() => {
      this.showConfirmDelete = false;
    });
  }

  delete() {
    this.itemsService.delete(this.item.id).subscribe();
  }

  registerAutosave() {
    this.itemForm.valueChanges
      .pipe(debounceTime(600))
      .subscribe(_ => {

        if (checkFormInvalidAndSignalChange(this.itemForm))
          return;

        const obj = new Item(this.itemForm.getRawValue()) as Item;
        obj.id = this.item.id;
        obj.schemaId = this.item.schemaId;

        obj.properties.forEach(prop => {
          prop.value = prop.value.toString();
        });

        this.itemsService.update(this.item.id, obj).subscribe(response => {
          this.footerMessage = "Saved!";
          timer(1500).subscribe(() => {
            this.footerMessage = `Updated on: ${this.datePipe.transform(new Date(), "medium")}`;
          });
        }, (res) => {
          applyValidationErrorsToFormGroup(res.error, this.itemForm);
        });
      });
  }

  public getPropertyFieldType(propertyName: string) {

    const schemaPropertyType = this.schema.properties.find(prop => prop.name === propertyName)?.type;

    switch (schemaPropertyType) {
      case ItemSchemaPropertyType.Text:
        return "text";
      case ItemSchemaPropertyType.Number:
        return "number";
      case ItemSchemaPropertyType.Checkbox:
        return "checkbox";
      default:
        return "text";
    }
  }

  public get properties(): FormArray {
    return this.itemForm.get("properties") as FormArray;
  }

  public get itemSchemaPropertyTypes() {
    return ItemSchemaPropertyType;
  }

  get itemType() {
    return this.schema ? this.schema.name : "Untyped";
  }
}
