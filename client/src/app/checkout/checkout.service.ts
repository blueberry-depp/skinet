import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {map} from "rxjs";
import {IDeliveryMethod} from "../shared/models/deliveryMethod";
import {IOrderToCreate} from "../shared/models/order";

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl

  constructor(private http: HttpClient) { }

  createOrder(order: IOrderToCreate) {
    return this.http.post(`${this.baseUrl}orders`, order)

  }

  // We didn't do anything to sort these on the server. So we'll sort these on the client and there's only four of them. So it's no big deal.
  getDeliveryMethods() {
    // In order to sort them we're gonna need to use the pipe operator and map this into whatever we want.
    return this.http.get(`${this.baseUrl}orders/deliveryMethods`).pipe(map(
      (deliveryMethod: IDeliveryMethod[])  => {
        // Return in sorted format. Sort it to the highest price first, this is going to guarantee they're sorted in highest price first which,
        // possibly is what we're going to return anyway from the API. But this is just to make sure that we get them how we want them.
        return deliveryMethod.sort((a, b) => b.price - a.price)
      }
    ))
  }



}
