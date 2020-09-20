import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemSchemaComponent } from './item-schema.component';

describe('ItemSchemaComponent', () => {
  let component: ItemSchemaComponent;
  let fixture: ComponentFixture<ItemSchemaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemSchemaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemSchemaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
