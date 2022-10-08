import {Component, Input, OnInit} from '@angular/core';
import {FormGroup} from "@angular/forms";
import {CheckoutService} from "../checkout.service";
import {IDeliveryMethod} from "../../shared/models/deliveryMethod";
import {BasketService} from "../../basket/basket.service";

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup
  deliveryMethods: IDeliveryMethod[]


  constructor(
    private checkoutService: CheckoutService,
    private basketService: BasketService,

  ) { }

  ngOnInit(): void {
    this.checkoutService.getDeliveryMethods().subscribe({
      // Getting back deliveryMethod which is a type of IDeliveryMethod array.
      next: (deliveryMethod: IDeliveryMethod[]) => {
        this.deliveryMethods = deliveryMethod
      },
      error: error => console.log(error)
    })
  }

  // We now can add a click event to radio buttons so that when we do click on one of the methods we call this particular
  // method and we update totals and it's going to be reflected on summary.
  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.basketService.setShippingPrice(deliveryMethod)
  }



}
