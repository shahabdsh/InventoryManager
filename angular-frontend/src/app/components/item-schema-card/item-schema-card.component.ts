import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { ItemSchema, ItemSchemaPropertyType } from "@models/item-schema";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ItemSchemaService } from "@services/item-schema.service";
import { Item } from "@models/item";
import { DatePipe } from "@angular/common";
import { timer } from "rxjs";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { checkAndSendForm } from "@utils/check-and-send-form";

@Component({
  selector: "app-item-schema-card",
  templateUrl: "./item-schema-card.component.html",
  styleUrls: ["./item-schema-card.component.scss"]
})
export class ItemSchemaCardComponent implements OnInit {

  @ViewChild("errorModal", {static: true}) errorModal;

  @Input() schema: ItemSchema;
  schemaForm: FormGroup;

  changed: boolean;

  footerMessage: string;

  showConfirmDelete: boolean;

  // Todo: Automatically init this.
  itemPropertyTypesArray = [
    {label: "Text", value: ItemSchemaPropertyType.Text},
    {label: "Number", value: ItemSchemaPropertyType.Number},
    {label: "Checkbox", value: ItemSchemaPropertyType.Checkbox},
  ];

  constructor(private fb: FormBuilder,
              private itemSchemaService: ItemSchemaService,
              private datePipe: DatePipe,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {

    const properties = this.schema.properties.map(prop => {
      return this.fb.group({
        name: [prop.name, Validators.required],
        type: prop.type
      });
    });

    this.schemaForm = this.fb.group({
      name: [this.schema.name, Validators.required],
      properties: this.fb.array(properties),
    });

    this.schemaForm.valueChanges.subscribe(_ => {
      this.changed = true;
    });

    this.changed = false;

    if (this.schema.updatedOn) {
      this.footerMessage = `Updated on: ${this.datePipe.transform(this.schema.updatedOn, "medium")}`;
    } else if (this.schema.createdOn) {
      this.footerMessage = `Created on: ${this.datePipe.transform(this.schema.createdOn, "medium")}`;
    }
  }

  createNewProperty(): FormGroup {
    return this.fb.group({
      name: ["", Validators.required],
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
    return this.schemaForm.get("properties") as FormArray;
  }

  getTypeLabel(typeValue) {
    return this.itemPropertyTypesArray.find(prop => prop.value === typeValue).label;
  }

  save() {

    this.changed = false;

    checkAndSendForm(this.schemaForm, () => {
      const obj = new Item(this.schemaForm.value) as any;
      obj.id = this.schema.id;

      return this.itemSchemaService.update(this.schema.id, obj);
    }, () => {
      this.footerMessage = "Saved!";
      timer(1500).subscribe(() => {
        this.footerMessage = `Updated on: ${this.datePipe.transform(new Date(), "medium")}`;
      });
    }, () => {
      this.changed = true;
    });
  }

  attemptDelete() {
    this.showConfirmDelete = true;

    timer(2000).subscribe(() => {
      this.showConfirmDelete = false;
    });
  }

  delete() {
    this.itemSchemaService.delete(this.schema.id).subscribe(() => {
    }, error => {
      this.showDeleteErrorModal(this.errorModal);
    });
  }

  showDeleteErrorModal(content) {
    this.modalService.open(content, {ariaLabelledBy: "modal-basic-title"}).result.then((result) => {

    }, reason => {

    });
  }
}
