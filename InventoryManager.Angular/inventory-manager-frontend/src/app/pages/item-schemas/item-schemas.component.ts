import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Item } from '../../models/item';
import {ItemSchemaService} from '../../services/item-schema.service';
import {ItemSchema} from '../../models/item-schema';

@Component({
  selector: 'app-item-schemas',
  templateUrl: './item-schemas.component.html',
  styleUrls: ['./item-schemas.component.scss']
})
export class ItemSchemasComponent implements OnInit {

  schemas: Observable<ItemSchema[]>;

  constructor(private itemSchemaService: ItemSchemaService) { }

  ngOnInit(): void {
    this.schemas = this.itemSchemaService.getAll();
  }
}
