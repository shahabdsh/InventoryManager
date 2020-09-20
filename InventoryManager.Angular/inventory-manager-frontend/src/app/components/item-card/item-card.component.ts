import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../models/item';
import {FormBuilder, FormGroup} from '@angular/forms';
import { ItemService } from '../../services/item.service';
import {debounceTime} from 'rxjs/operators';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.scss']
})
export class ItemCardComponent implements OnInit {

  @Input() item: Item;
  itemForm: FormGroup;

  constructor(private fb: FormBuilder, private itemsService: ItemService) { }

  ngOnInit(): void {

    this.itemForm = this.fb.group({
      name: [this.item.name],
      type: [this.item.type],
      quantity: [this.item.quantity],
    });

    this.itemForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(_ => {

        const obj = new Item(this.itemForm.value) as any;
        obj.id = this.item.id;

        this.itemsService.updateItem(this.item.id, obj).subscribe();
      });
  }
}
