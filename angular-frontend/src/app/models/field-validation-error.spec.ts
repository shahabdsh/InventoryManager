import { FieldValidationError } from "./field-validation-error";

describe("ValidationError", () => {
  it("should create an instance", () => {
    expect(new FieldValidationError()).toBeTruthy();
  });
});
