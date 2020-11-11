import { Component, HostBinding, Input, OnInit } from "@angular/core";
import { AbstractControl } from "@angular/forms";

@Component({
  selector: "[app-validation-errors]",
  templateUrl: "./validation-errors.component.html",
  styleUrls: ["./validation-errors.component.scss"]
})
export class ValidationErrorsComponent implements OnInit {

  @Input("app-validation-errors") control: AbstractControl;

  @HostBinding('class.invalid-feedback') invalidFeedbackClassBinding: boolean = false;

  defaultErrorMessages = {
    required: "Field is required"
  };

  constructor() {
    this.invalidFeedbackClassBinding = true;
  }

  ngOnInit(): void {
  }

  public get errors() {

    let errorsList = [];
    let errors = this.control.errors;

    for (const property in errors) {
      if (errors.hasOwnProperty(property)) {
        if (property === "serverErrors") {
          errorsList = errorsList.concat(errors[property]);
        } else if (this.defaultErrorMessages[property]) {
          errorsList.push(this.defaultErrorMessages[property]);
        } else {
          errorsList.push(property);
        }
      }
    }

    return errorsList;
  }
}
