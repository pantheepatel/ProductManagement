export class InvoiceModel {
  constructor(
    public id: string,
    public customerId: string,
    public date: Date,
    public grandTotal: number,
    public taxTotal: number,
    public priceTotal: number,
    public totalItems: number = 0,
    public products: InvoiceItemModel[] = [] // array of invoice items
  ) { }
}
export class InvoiceItemModel {
  constructor(
    public id: string,
    public invoiceId: string,
    public productId: string,
    public productName: string,
    public quantity: number,
    public unitPrice: number,
    public subTotal: number,
    public tax: number = 0,
    public taxTotal: number = 0,
    public total: number // total price for this item (quantity * (price + tax))
  ) { }
}

export class InvoiceCreateDTO {
  constructor(
    public customerId: string,
    public date: Date,
    public products: InvoiceItemCreateDTO[]
  ) { }
}
export class InvoiceItemCreateDTO {
  constructor(
    public productId: string,
    public quantity: number
  ) { }
}
