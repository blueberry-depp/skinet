// Use this for all of order related things.
import {IAddress} from "./address";

// All of this information is available even in one of existing services for the basket ID and deliveryMethodId and shipToAddress,
// we get this information from the checkout form.
export interface IOrderToCreate {
  basketId: string;
  deliveryMethodId: number;
  shipToAddress: IAddress;
}

export interface IOrder {
  id: number;
  buyerEmail: string;
  orderDate: string;
  shipToAddress: IAddress;
  deliveryMethod: string;
  shippingPrice: number
  orderItems: IOrderItem[];
  subtotal: number;
  status: number;
}

export interface IOrderItem {
  productId: number;
  productName: string;
  pictureUrl: string;
  price: number;
  quantity: number
}


