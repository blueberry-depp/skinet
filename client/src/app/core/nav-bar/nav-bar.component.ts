import { Component, OnInit } from '@angular/core';
import {BasketService} from "../../basket/basket.service";
import {Observable} from "rxjs";
import {IBasket} from "../../shared/models/basket";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  basket$: Observable<IBasket>

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    // Get the observable from the service.
    this.basket$ = this.basketService.basket$
  }

}
