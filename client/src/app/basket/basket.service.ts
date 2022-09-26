import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {BehaviorSubject, map} from "rxjs";
import {Basket, IBasket, IBasketItem, IBasketTotals} from "../shared/models/basket";
import {IProduct} from "../shared/models/product";

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;

  // We need access to basket information inside the basket icon top here for the shopping cart. This is going to need to display
  // the number of items. We're also going to need basket information  basket component and there's going to be other areas we need
  // the information inside the basket as well, and what we're going to do is we're going to create a private property.
  // new BehaviorSubject(): which is a variant of subject which is an observable that requires an initial value. And emit its current
  // value whenever it is subscribed to. So we're going to be able to subscribe to this and because it's a
  // behavior subject then it's always going to emit an initial value. We give the initial value is null.
  private basketSource = new BehaviorSubject<IBasket>(null)
  // Because basketSource is private we're going to need is a public property that's going to be accessible by other
  // components in our application. And now we were able to subscribe using the async pipe in order to get the value because it's observable.
  basket$ = this.basketSource.asObservable()

  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null)
  basketTotal$ = this.basketTotalSource.asObservable()

  constructor(private http: HttpClient) {
  }

  // We're not subscribing to observable we get back from http client at this moment in time. What we're going to use to subscribe
  // is the async pipe that will make use of in the components that connect to this basket observable.
  getBasket(id: string) {
    return this.http.get(`${(this.baseUrl)}basket?id=${id}`)
      // Now what we want to do is from the response that we get back from the http client which should contain basket then we
      // need to set our basket source with the basket we get back from the API. To achieve that we use the pipe rxjs method
      .pipe(map((basket: IBasket) => {
        // In order to set the BehaviorSubject next property or next value then we've got a method inside our BehaviorSubject called next.
        this.basketSource.next(basket)
        // After we've got the baskets from the API, we set the totals.
        this.calculateTotals()
      }))
  }

  // We will subscribe to because we want this to execute whatever we set the basket.
  setBasket(basket: IBasket) {
    return this.http.post(`${(this.baseUrl)}basket`, basket).subscribe({
      next: (response: IBasket) => {
        // This going to update BehaviorSubject with the new value of the basket that we have.
        this.basketSource.next(response)
        this.calculateTotals()
      }, error: error => {
        console.log(error)
      }
    })
  }


  // To easily get access to what's inside basketSource to get the current value without actually subscribing to anything.
  getCurrentBasketValue() {
    // Whatever contained inside the basketSource whether it's null or whether it's basket it's going to give us the value of this when we call this method.
    return this.basketSource.value
  }


  // Because we're going to be adding them from our product component, is going to be a type of IProduct.
  addItemToBasket(item: IProduct, quantity = 1) {
    // We create a method to map the properties from the product to the basket item.
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity)
    // This is even going to return null or it's going to return of a basket. And if it returns null then we can use the null
    // coalescing operator and creates baskets.
    //const basket = this.getCurrentBasketValue() ?? this.createBasket()
    let basket = this.getCurrentBasketValue()
    if (basket === null) {
      basket = this.createBasket()
    }
    // Check to see if there's already an item of this product in a basket and if there is then we just increase the quantity
    // by whatever this quantity is set to or if we don't already have an item of this product then we're just going to push an additional item into the basket.
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity)
    this.setBasket(basket)
  }


  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue()
    // Check to see if there's an existing item of this product in the baskets already.
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id)
    // console.log('Increment Item Index', foundItemIndex)
    basket.items[foundItemIndex].quantity++
    this.setBasket(basket)
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue()
    // Check to see if there's an existing item of this product already in the baskets.
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id)
    // console.log('Decrement Item Index', foundItemIndex)
    // Check here to see if the quantity that we currently have is greater than 1 if it is then we're just going to decrement the quantity by 1
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--
      // console.log('Decrement Item Index', basket.items[foundItemIndex])
      this.setBasket(basket)
    } else {
      // But if it's not greater than one then we're going to want to remove the item in its entirety from the basket.
      console.log('Got you')
      this.removeItemFromBasket(item)
    }
  }

  // Check to see if there's any other items in the basket and if there aren't then we're going to delete the basket in its entirety.
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue()
    // This is going to return a boolean to see if there's any items in the basket that match this particular ID.
    if (basket.items.some(x => x.id === item.id)) {
      // Check to see if we have or what we'll return from this filter method is all of the other items that do not match this particular ID,
      // this is going to return an array that's going to populate inside basket items of all of the items except for the one that matches this ID.
      basket.items = basket.items.filter(i => i.id !== item.id)
      // Check to see if the basket items length is greater than zero.
      if (basket.items.length > 0) {
        // And if it is then we're just going to set the basket with the current items.
        this.setBasket(basket)
      } else {
        // If this was the only item remaining in the baskets and we've removed it we now have an empty basket and we might as well delete the basket.
        this.deleteBasket(basket)
      }
    }
  }

  private deleteBasket(basket: IBasket) {
    return this.http.delete(`${(this.baseUrl)}basket?id=${basket.id}`).subscribe({
      next: () => {
        this.basketSource.next(null)
        this.basketTotalSource.next(null)
        localStorage.removeItem('basket_id')
      },
      error: error => console.log(error)
    })
  }


  // create a private method inside this class to calculate the totals from what's inside basket and add them to this basketTotalSource behaviour subject. So
  // we set the totals there.
  private calculateTotals() {
    // Get current basket.
    const basket = this.getCurrentBasketValue()
    const shipping = 0
    // The basket items is an array of items and some of them may have more than one in terms of the quantity and what we want to do is sum
    // these values together and reduce that into a single number that we can put into subtotal,
    // and one of the array methods is to reduce the array and what this does it call the specified callback function for all the elements in an array,
    // we've got an array of basket items and the return value of the callback function is the accumulated result and our accumulator
    // function is going to add the item * quantity for each item and it's going to sum all of those and it's just going to return a single value for the quantity.
    // b: represents the item and each item has a price and the quantity. And we're multiplying that together and then we're adding it to 'a'.
    // a: represents the results that we're returning from this reduce function, 'a' is given an initial value here of zero, so we start at zero
    // we go to item 1 we times its price by its quantity and then we add it to 'a' which becomes whatever that is. And then we do this for each item
    // in list of items. And it gives us subtotal.
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0)
    const total = shipping + subtotal
    // Set the next value of this observable. And this is going to be IBasketTotals
    this.basketTotalSource.next({shipping, subtotal, total})
  }

  // Check to see if there's already an item of this product in particular baskets,
  // now each item is going to have the product ID as its ID because we were setting this in the mapping(mapProductItemToBasketItem) earlier on,
  // so all we need to do is look for the product item ID see if we've already got an item in basket that matches that particular ID. And if we do
  // then we know that we're just increasing the quantity. If not then we're going to push a new item into our list.
  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    // console.log(items)
    const index = items.findIndex(i => i.id === itemToAdd.id)
    if (index === -1) {
      itemToAdd.quantity = quantity
      items.push(itemToAdd)
    } else {
      // items is an array and in order to access the elements at what's ever this index is in the array then we can just pass in the index.
      // quantity: the quantity it currently is set and we're going to add whatever the quantity is on top of that as well.
      items[index].quantity += quantity
    }
    return items
  }

  private createBasket(): IBasket {
    // this is going to create a basket with an ID which is going to be a unique identifier for this basket and initialize it with an empty list of items as well.
    const basket = new Basket()
    // We want to persist the basket in a way on the client, is we want to store the basket ID somewhere so that when
    // we load application or when the user comes into application we've got the ID of the baskets, and we
    // retrieve it from API when the application starts up if they have a basket already,
    // so we'll use local storage on the client's browser to store the baskets ID once it's been created,
    // this is going to give us some level of persistence as long as the user doesn't clear out their local
    // storage then we'll be able to go and retrieve their baskets even if they closed their browser, restart their computer. We'll be able to retrieve
    // this basket ID because local storage persists even after the browser is closed down or the computer is restarted,
    // this local storage very specific to a browser. So if a user uses chrome and adds items into the basket and
    // that opens up Firefox and then they're not going to see the baskets, this is just specific to an individual browser but each browser has local
    // storage available for us to use, and we can store the basket ID inside their.
    localStorage.setItem('basket_id', basket.id)
    return basket
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      // Create the object simply map the properties from the product into the basket item.
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    }
  }



}
