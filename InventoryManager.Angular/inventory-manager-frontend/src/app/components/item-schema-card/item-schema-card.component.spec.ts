import { ComponentFixture, TestBed } from "@angular/core/testing";

import { ItemSchemaCardComponent } from "./item-schema-card.component";

describe("ItemSchemaComponent", () => {
  let component: ItemSchemaCardComponent;
  let fixture: ComponentFixture<ItemSchemaCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemSchemaCardComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemSchemaCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
