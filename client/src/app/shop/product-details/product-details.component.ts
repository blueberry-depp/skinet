import {Component, OnInit} from '@angular/core';
import {IProduct} from "../../shared/models/product";
import {ShopService} from "../shop.service";
import {ActivatedRoute} from "@angular/router";
import {BreadcrumbService} from "xng-breadcrumb";
import {BasketService} from "../../basket/basket.service";

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct
  quantity = 1

  constructor(
    private shopService: ShopService,
    // To get access to root parameter here, we need to inject something from the router module and it is the activated root.
    private activateRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService
) {
    this.breadcrumbService.set('@productDetails', ' ')
  }

  ngOnInit(): void {
    this.loadProduct()
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this.quantity)
  }

  decrementQuantity() {
    if (this.quantity > 1) this.quantity--
  }

  incrementQuantity() {
    this.quantity++
  }

  loadProduct() {
    // +: cast to int/number
    this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe({
      next: product => {
        this.product = product
        this.breadcrumbService.set('@productDetails', product.name)
      }, error: error => {
        console.log(error)
      }
    })
  }

}
