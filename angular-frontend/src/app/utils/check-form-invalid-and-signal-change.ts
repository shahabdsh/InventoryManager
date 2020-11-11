import { FormGroup } from "@angular/forms";

export function checkFormInvalidAndSignalChange(fg: FormGroup) {

  if (!fg.valid)
    fg.patchValue(fg.value);

  return !fg.valid;
}

