import { v4 as uuidv4 } from 'uuid';

export interface IBasket {
  id: string;
  items: IBasketItem[];
}

export interface IBasketItem {
  id: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: string;
  type: string;
}

// When we create a new basket, we want to give it a unique identifier and empty array of items.
export class Basket implements IBasket {
    id = uuidv4()
    // Initialize an empty array for findIndex undefined solution.
    items: IBasketItem[] = [];
}

export interface IBasketTotals {
  shipping: number
  subtotal: number
  total: number
}
