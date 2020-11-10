import { Component, OnInit } from "@angular/core";
import { ItemService } from "@services/item.service";
import { ItemSchemaService } from "@services/item-schema.service";
import { ItemSchema } from "@models/item-schema";
import { Item } from "@models/item";
import { FormBuilder, FormGroup } from "@angular/forms";
import { debounceTime } from "rxjs/operators";
import { BehaviorSubject} from "rxjs";

@Component({
  selector: "app-all-items",
  templateUrl: "./all-items.component.html",
  styleUrls: ["./all-items.component.scss"]
})
export class AllItemsComponent implements OnInit {

  searchForm: FormGroup;

  filteredItems$: BehaviorSubject<Item[]>;

  constructor(public itemsService: ItemService,
              private fb: FormBuilder,
              private itemSchemaService: ItemSchemaService) {

    this.filteredItems$ = new BehaviorSubject<Item[]>(null);
  }

  ngOnInit(): void {

    this.searchForm = this.fb.group({
      search: []
    });

    this.itemsService.allEntities$.subscribe(items => {
      this.filteredItems$.next(items);
    })

    this.registerSearchCallback();

    this.itemsService.getAllIfCurrentEntitiesAreNull();
    this.itemSchemaService.getAllIfCurrentEntitiesAreNull();
  }

  addItem(schema: ItemSchema) {
    this.itemsService.createUsingSchema(schema).subscribe();
  }

  get schemas$() {
    return this.itemSchemaService.allEntities$;
  }

  itemTrackBy(index: number, item: Item) {
    return item.id;
  }

  registerSearchCallback () {
    this.searchForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(val => {

        let searchTerm = val.search;

        this.itemsService.allEntitiesTakeOne
          .subscribe((currentItems) => {

            if (searchTerm) {
              this.itemsService.getIds(searchTerm).subscribe(ids => {

                let newItems = currentItems.filter(item => {
                  return ids.indexOf(item.id) !== -1;
                });

                this.filteredItems$.next(newItems);
              }, () => {
                this.filteredItems$.next(currentItems);
                this.searchForm.controls["search"].setErrors({"invalid": true});
              })
            } else {
              this.filteredItems$.next(currentItems);
            }
        });
      });
  }
}
