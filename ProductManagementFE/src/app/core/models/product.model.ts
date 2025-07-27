import { ProductPriceModel } from "./productPrice.model";

export class ProductModel {
  constructor(
    public id: string,
    public name: string,
    public description: string,
    public basePrice: number, // if no seasonal price is set, this is the base price
    public categoryId: string,
    public tax: number = 0,
    public currentPrice?: number, // current price after applying tax
    public prices?: any[]// array of seasonal prices
  ) { }
}
export class ProductCreateDTO {
  constructor(
    public name: string,
    public description: string,
    public basePrice: number, // if no seasonal price is set, this is the base price
    public categoryId: string,
    public tax: number = 0,
    public prices?: any[]// array of seasonal prices
  ) { }
}