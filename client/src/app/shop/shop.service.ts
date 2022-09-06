import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {IPagination} from "../shared/models/pagination";
import {IBrand} from "../shared/models/brand";
import {IType} from "../shared/models/productType";
import {map} from "rxjs";
import {ShopParams} from "../shared/models/shopParams";

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl
  shopParams = new ShopParams()


  constructor(private http: HttpClient) {
  }

  getShopParams() {
    return this.shopParams
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params
  }

  // shopParams: ShopParams: typescript classes can be used as classes themselves that we can create new instances of but we can also use them as types.
  getProducts(shopParams: ShopParams) {
    // Create a params object that we can pass up to API.
    let params = new HttpParams()

    // Because brandId and typeId initialized to 0 in shop parameters class, we actually don't want to send up a
    // query string way of zero as the brand id or type id. because that's not going to exist on our server,
    // so we just check to make sure it's not equal to zero and only send up this parameter if that is the case because we're initializing
    // it from the class now instead of initializing it from inside the component.
    if (shopParams.brandId !== 0) {
      // toString(): this has to be a string that goes up as part of url which is obviously already a string.
      params = params.append('brandId', shopParams.brandId.toString())
    }

    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString())
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search)
    }

    // We can also take out the check because we're going to start with sorting it by name by default.
    params = params.append('sort', shopParams.sort)
    // Add pagination to parameters, pageIndex as the key of the parameter.
    params = params.append('pageIndex', shopParams.pageNumber.toString())
    params = params.append('pageSize', shopParams.pageSize.toString())


    // Pass in an object and inside this object we want to observe the response.
    // We're observing a response, this is going to give us the http response instead of the body of the response,
    // we actually need to project this data into actual response. We need to extract the body out of this,
    // we can manipulate this observable and project it into an IPagination object. And what we want to do is get
    // the body of the response and projects that into the IPagination object, then we use pipe,
    // in order to use rxjs methods then we need to pipe the response into something. Inside this pipe we can make use of rxjs methods,
    // we have an http response at the moment, and we want to map this into an IPagination object.
    return this.http.get<IPagination>(`${(this.baseUrl)}products`, {observe: 'response', params})
      // Take our response that we get back from the API
      // Inside this pipe method we can chain as many rxjs operators together to manipulate or do something with the observable as it comes back in.
      .pipe(map(response => {
        // The response.body is going to be IPagination object.
        return response.body
      }))
  }

  getBrands() {
    return this.http.get<IBrand[]>(`${(this.baseUrl)}products/brands`)
  }

  getTypes() {
    return this.http.get<IType[]>(`${(this.baseUrl)}products/types`)
  }
}
