// We can actually use a class in this particular occasion because a class gives us
// an opportunity to initialize the values when we create a new instance of it.
export class ShopParams {
  brandId = 0
  typeId = 0
  sort = 'name'
  pageNumber = 1
  pageSize = 6
  search: string
}
