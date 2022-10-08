import {Component, Input, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {BasketService} from "../../basket/basket.service";
import {CheckoutService} from "../checkout.service";
import {ToastrService} from "ngx-toastr";
import {IBasket} from 'src/app/shared/models/basket';
import {IOrderToCreate} from "../../shared/models/order";
import {NavigationExtras, Router} from "@angular/router";

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup

  // To get the basket ID, we need basket service and we'll also need checkout service
  // so that we can get the access to the create order method.
  constructor(
    private checkoutService: CheckoutService,
    private basketService: BasketService,
    private toastr: ToastrService,
    private router: Router,

  ) {
  }

  ngOnInit(): void {
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue()
    const orderToCreate = this.getOrderToCreate(basket)
    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: (order: IOrderToCreate) => {
        this.toastr.success('Order created successfully')
        this.basketService.deleteLocalBasket(basket.id)
        // Because we've getting our order back from API. We can pass order to order success page
        // purely so that we can use the ID from the order to allow them to click on a button to go and view the details of that order.
        const navigationExtras: NavigationExtras = {state: order}
        this.router.navigate(['checkout/success'], navigationExtras)
        // console.log(order)
      },
      error: err => {
        this.toastr.error(err.message)
        console.log(err)
      }
    })

  }

  private getOrderToCreate(basket: IBasket) {
    return {
      // Get the things that we want to add to make an order, that matches orderDto.
      basketId: basket.id,
      // +: make it number.
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    }
  }

}
