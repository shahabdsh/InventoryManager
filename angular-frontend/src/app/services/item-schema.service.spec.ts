import { TestBed } from "@angular/core/testing";

import { ItemSchemaService } from "./item-schema.service";

describe("ItemSchemaService", () => {
  let service: ItemSchemaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemSchemaService);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
