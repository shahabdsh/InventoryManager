import { FieldValidationError } from "@models/field-validation-error";
import { FormGroup } from "@angular/forms";
import { camelize } from "@utils/camelize";

export function applyValidationErrorsToFormGroup(errors: FieldValidationError[], fg: FormGroup) {

  errors.forEach(error => {

    const formControlName = camelize(error.propertyName);
    const formControl = fg.get(formControlName);
    if (formControl) {

      let controlErrors = formControl.errors ?? {};

      controlErrors.serverErrors = [error.errorMessage];

      formControl.setErrors(controlErrors);
    }
  });
}

