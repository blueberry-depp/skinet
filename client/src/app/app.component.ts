import {Component, OnInit} from '@angular/core';
import {BasketService} from "./basket/basket.service";
import {logMessages} from "@angular-devkit/build-angular/src/builders/browser-esbuild/esbuild";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Skinet';

  // Now that we have items being added to baskets and we have the baskets ID stored in our local storage inside browser,
  // and what we want to do is when our application starts up, what we want to do is get our baskets and
  // we want to check to see if we've got the basket ID in local storage. And if we have, we'll go and fetch our basket from the API,
  // so a good place to do any initialization is in roots component for our application. And that's app component.
  constructor(private basketService: BasketService
  ) {
  }


  ngOnInit(): void {
    // Check inside local storage to see if we've got a basket, if we have get the basket ID from localstorage
    const basketId = localStorage.getItem('basket_id')
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe({
        next: () => console.log('initialised basket'),
        error: error => console.log(error)
      })
    }
  }
}
