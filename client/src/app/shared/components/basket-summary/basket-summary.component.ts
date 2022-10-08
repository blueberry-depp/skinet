import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BasketService} from "../../../basket/basket.service";
import {Observable} from "rxjs";
import {IBasket, IBasketItem, IBasketTotals} from "../../models/basket";
import * as events from "events";
import {IOrderItem} from "../../models/order";

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  basket$: Observable<IBasket>
  @Output() decrement = new EventEmitter<IBasketItem>()
  @Output() increment = new EventEmitter<IBasketItem>()
  @Output() remove = new EventEmitter<IBasketItem>()
  @Input() items: IBasketItem[] | IOrderItem = []

  // In basket summary we'll also take an input property that's just going to be a flag to say if it's the basket component
  // and if it's not a basket component then we don't want to see any of these options.
  @Input() isBasket = true

  constructor(
    private basketService: BasketService,
  ) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$
  }

  decrementItemQuantity(item: IBasketItem ) {
    this.decrement.emit(item)
  }

  incrementItemQuantity(item: IBasketItem ) {
    this.increment.emit(item)
  }

  removeBasketItem(item: IBasketItem ) {
    this.remove.emit(item)
  }



}
