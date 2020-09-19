import { Component, OnInit } from '@angular/core';
import { ItemsService } from "../../services/items.service";
import {Observable} from "rxjs";
import {Item} from "../../models/item";

@Component({
  selector: 'app-all-items',
  templateUrl: './all-items.component.html',
  styleUrls: ['./all-items.component.scss']
})
export class AllItemsComponent implements OnInit {

  items: Observable<Item[]>;

  constructor(private itemsService: ItemsService) { }

  ngOnInit(): void {
    this.items = this.itemsService.getAllItems();
  }

}
