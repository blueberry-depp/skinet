import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {IProduct} from "../shared/models/product";
import {ShopService} from "./shop.service";
import {IPagination} from "../shared/models/pagination";
import {IBrand} from "../shared/models/brand";
import {IType} from "../shared/models/productType";
import {ShopParams} from "../shared/models/shopParams";
import {repeat} from "rxjs";

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  // static: false: which means this element is going to appear after we have our products available.
  @ViewChild('search', {static: false}) searchTerm: ElementRef
  products: IProduct[]
  brands: IBrand[]
  types: IType[]
  shopParams = new ShopParams()
  totalCount: number
  // And the 'value', this is what we want to send up as the query string parameter.
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ]

  constructor(private shopService: ShopService) { }

  ngOnInit() {
   this.getProducts()
   this.getBrands()
   this.getTypes()
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.products = response.data
        this.shopParams.pageNumber = response.pageIndex
        this.shopParams.pageSize = response.pageSize
        this.totalCount = response.count
      },
      error: error => {
        console.log(error)
      }
    })
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => {
        // Make it also can reset the filter,
        // the response is an array of brand objects. And what we're doing is creating another object({id: 0, name: 'All'}) to add to this array,
        // and when we use spread operator it spreads all the objects from that array(response) and is simply adding on another objects
        // at the front here({id: 0, name: 'All'}) which is name: 'All'.
        this.brands = [{id: 0, name: 'All'}, ...response]
      },
      error: error => {
        console.log(error)
      }
    })
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => {
        this.types = [{id: 0, name: 'All'}, ...response]
      },
      error: error => {
        console.log(error)
      }
    })
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId
    // We're need to return the first page of those results regardless from our server.
    this.shopParams.pageNumber = 1
    this.getProducts()
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId
    this.shopParams.pageNumber = 1
    this.getProducts()
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort
    this.getProducts()
  }

  onPageChanged(event: any) {
    // The problem: causing two time call similar api at one time.
    // Changing totalItems input property also causes pageChanged in pager template to be fired even though
    // we don't want it to be fired because if we take a look at component then in onPageChanged events we're going to call getProducts,
    // but if we change one of the filters then we're also calling getProducts, we don't want pageChanged in pager template
    // to be called if we're clicking on one of filters so to prevent we wrap with if statement,
    // we only want onPageChanged event if we're actually changing page and the easiest way to do that is just to compare the page number
    // with the events which is another page of course. And if it's different then we know we're going from page one to two or two to three
    // and then we can call this particular method.
    if (this.shopParams.pageNumber !== event) {
      // We don't need to add event.page anymore because that's being supplied by the child component.
      this.shopParams.pageNumber = event
      this.getProducts()
    }

  }

  onSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value
    this.shopParams.pageNumber = 1
    this.getProducts()
  }

  onReset() {
    this.searchTerm.nativeElement.value = ''
    // We use this to reset all filters to default value.
    this.shopParams = new ShopParams()
    // Get unfiltered product
    this.getProducts()
  }

}
