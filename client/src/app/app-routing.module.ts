import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./home/home.component";
import {TestErrorComponent} from "./core/test-error/test-error.component";
import {NotFoundComponent} from "./core/not-found/not-found.component";
import {ServerErrorComponent} from "./core/server-error/server-error.component";
import {AuthGuard} from "./core/guards/auth.guard";

const routes: Routes =  [

  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  {path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'}},

  // This is how we deal with lazy loading. Now shop module is only going to be activated and loaded when we access the shop path.
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule), data: {breadcrumb: 'Shop'}},
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule), data: {breadcrumb: 'Basket'}},
  {path: 'checkout', canActivate: [AuthGuard], loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule), data: {breadcrumb: 'Checkout'}},
  // We set breadcrumb to skip = true, because we're not going to have anything routing to this particular level. But the path when somebody goes to
  // login or register will include account/login or account/register.
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}}},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
