import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {ShopComponent} from "./shop.component";
import {ProductDetailsComponent} from "./product-details/product-details.component";

const routes: Routes = [
  // We don't need the path to the shop inside this shop route in component.
  {path: '', component: ShopComponent},
  {path: ':id', component: ProductDetailsComponent, data: {breadcrumb: {alias: 'productDetails'}}},
]

@NgModule({
  declarations: [],
  imports: [
    // We want these routes to be loaded up for child and that means these routes are not available in app module and
    // are only going to be available in shop module.
    RouterModule.forChild(routes)
  ],
  // Export the router module because we're want to use this router module inside shop module.
  exports: [
    RouterModule
  ]
})
export class ShopRoutingModule {
}
