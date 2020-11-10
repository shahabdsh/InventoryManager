import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AllItemsComponent } from "@pages/all-items/all-items.component";
import { ItemSchemasComponent } from "@pages/item-schemas/item-schemas.component";

const routes: Routes = [
  {path: "all-items", component: AllItemsComponent},
  {path: "item-schemas", component: ItemSchemasComponent},
  {path: "", redirectTo: "/all-items", pathMatch: "full"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
