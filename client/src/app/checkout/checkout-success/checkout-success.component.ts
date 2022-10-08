import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {IOrder} from "../../shared/models/order";

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent implements OnInit {
  order: IOrder

  constructor(
    private router: Router,
  ) {
    // We need to get navigation extras inside constructor. We won't have it if we try and do this inside ngOnInit.
    const navigation = this.router.getCurrentNavigation()
    // Defensive again here, make sure we have this.
    const state = navigation && navigation.extras && navigation.extras.state
    // If we have an order.
    if (state) {
      // We name as IOrder so this order doesn't complain about states because we know we're setting this to an order.
      this.order = state as IOrder
      // if we don't have an order here instead of navigating the user to or letting them navigate to the individual order we'll just change it. So
      // they go to the orders page instead.
    }

  }

  ngOnInit(): void {
  }

}
