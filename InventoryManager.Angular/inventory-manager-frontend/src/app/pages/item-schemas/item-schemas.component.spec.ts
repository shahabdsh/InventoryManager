import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemSchemasComponent } from './item-schemas.component';

describe('ItemSchemasComponent', () => {
  let component: ItemSchemasComponent;
  let fixture: ComponentFixture<ItemSchemasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemSchemasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemSchemasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
