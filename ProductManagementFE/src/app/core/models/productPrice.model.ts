export class ProductPriceModel {
  constructor(
    public id: string,
    public productId: string,
    public startDate?: Date, // dates are optional, can be used for seasonal products
    public endDate?: Date,
    public seasonalPrice?: number, // if seasonal price is set, this will override the base price
  ) { }
}