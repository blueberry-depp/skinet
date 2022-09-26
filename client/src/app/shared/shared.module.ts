import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PaginationModule} from "ngx-bootstrap/pagination";
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import {CarouselModule} from "ngx-bootstrap/carousel";
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent
  ],
  imports: [
    CommonModule,
    // We need to add forRoot here as the pagination module has its own provider's array and those providers
    // need to be injected into our routes module at startup. So this is effectively acting as a singleton anyway. And if we
    // take off the four routes, then it won't load with its providers and will have errors.
    PaginationModule.forRoot(),
    CarouselModule.forRoot()
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    // Because we want to use this in a different module, we also need to export this module.
    OrderTotalsComponent
  ]
})
export class SharedModule { }
